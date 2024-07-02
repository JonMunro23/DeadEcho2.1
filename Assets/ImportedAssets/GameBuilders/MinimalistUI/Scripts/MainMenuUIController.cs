using GameBuilders.FPSBuilder.Core.Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

#pragma warning disable CS0649

namespace GameBuilders.MinimalistUI.Scripts
{
    public class MainMenuUIController : MonoBehaviour
    {
        [SerializeField]
        private BlackScreen m_PauseBlackScreen;
       

        private InputActionMap m_MenuInputBindings;

        int levelToLoadIndex;

        private void Start()
        {

            m_MenuInputBindings = GameplayManager.Instance.GetActionMap("Menu");
            m_MenuInputBindings.Enable();

        }

        public void StartLevel(int _levelToLoadIndex)
        {
            levelToLoadIndex = _levelToLoadIndex;
            m_MenuInputBindings.Disable();
            Time.timeScale = 1;
            m_PauseBlackScreen.Show = true;
            Invoke(nameof(LoadLevel), 1f);
        }

        private void LoadLevel()
        {
            SceneManager.LoadScene(levelToLoadIndex);
        }

        public void Quit ()
        {
            AudioListener.pause = false;
            Application.Quit();

            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }

        private void OnApplicationQuit ()
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
        }
        
        private static void HideCursor (bool hide)
        {
            if (hide)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
