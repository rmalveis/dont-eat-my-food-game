using System.Linq;
using System.Text;
using Players;
using UnityEngine;

namespace Prefab_Scripts
{
    public class InputName : ControlSetup
    {
        public int PlayerNumber = 1;
        public GameObject[] Chars;
        public GameObject InputYourNameObject;
        public UnityEngine.UI.Text CurrentMarker;

        private UnityEngine.UI.Text _current;

        private int _currentIndex;
        // Use this for initialization

        private void Start()
        {
            SetFocus();
            setController(PlayerNumber.ToString());
            var aux = InputYourNameObject.GetComponent<UnityEngine.UI.Text>().text;
            aux += " (" + ((int) PlayerType.Boss == PlayerNumber ? "Chefe" : "Empregado") + ")";

            InputYourNameObject.GetComponent<UnityEngine.UI.Text>().text = aux;
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetButtonDown(_horizontal))
            {
                SetFocus(Input.GetAxis(_horizontal) > 0);
            }
            else if (Input.GetButtonDown(_vertical))
            {
                SelectNextLetter(Input.GetAxis(_vertical) < 0);
            }
            else if (Input.GetButtonUp(_jump))
            {
                SaveName();
            }
        }

        private void SetFocus(bool next = false)
        {
            if (next)
            {
                _currentIndex = _currentIndex >= Chars.Length - 1 ? _currentIndex : _currentIndex + 1;
            }
            else
            {
                _currentIndex = _currentIndex == 0 ? _currentIndex : _currentIndex - 1;
            }

            _current = Chars[_currentIndex].GetComponent<UnityEngine.UI.Text>();
            var aux = _current.transform.position.x - CurrentMarker.transform.position.x ;
            CurrentMarker.transform.position = CurrentMarker.transform.position + new Vector3(aux, 0, 0);
            Debug.Log(CurrentMarker.transform.position);
        }

        private void SelectNextLetter(bool next)
        {
            var asciiBytes = Encoding.ASCII.GetBytes(_current.text);

            if (next)
            {
                if (asciiBytes[0] == 90)
                {
                    asciiBytes[0] = 65;
                }
                else
                {
                    asciiBytes[0]++;
                }
            }
            else
            {
                if (asciiBytes[0] == 65)
                {
                    asciiBytes[0] = 90;
                }
                else
                {
                    asciiBytes[0]--;
                }
            }

            _current.text = Encoding.ASCII.GetString(asciiBytes);
        }

        private void SaveName()
        {
            var name = Chars.Aggregate("", (current, c) => current + c.GetComponent<UnityEngine.UI.Text>().text);
            EventManager.EventManager.CallOnSave(name);
        }
    }
}