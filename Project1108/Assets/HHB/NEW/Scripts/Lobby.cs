using LEGACY;
using PlayFab.ClientModels;
using PlayFab;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class Lobby : MonoBehaviour
{
    public static NetworkManager Instance;
    [Header("로그인")]
    public TMP_InputField loginIdInput;
    public TMP_InputField loginPasswordInput;
    public TextMeshProUGUI loginInfoTxt;
    public Button loginButton;
    public Button quitGameButton;
    public Button registerButton;
    [Tooltip("로그인 UI")]
    public GameObject loginUI;

    [Header("회원가입")]
    public TMP_InputField signNickNameInput;
    public TMP_InputField signIdInput;
    public TMP_InputField signPassWordInput;
    public TextMeshProUGUI signInInfoTxt;
    public Button signButton;
    public Button backButton;
    [Tooltip("회원가입 팝업UI")]
    public GameObject signUI;

    [Header("캐릭터UI")]
    public GameObject characterUI;
    public GameObject character;

    private void Start()
    {
        PlayFabSettings.TitleId = "74971";
    }

    // 로그인
    public void Login()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = loginIdInput.text,
            Password = loginPasswordInput.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, (result) =>
        {
            loginInfoTxt.text = "LoginSuccess";
            loginUI.gameObject.SetActive(false);
            characterUI.gameObject.SetActive(true);
            CameraManager.Instance.SetCharacterCamera(character);
            //
            //Test test = GFUNC.FindTopLevelGameObject("Test").GetComponent<Test>();
            //test.InitDBWolfData();

            PlayerCustom player = GFUNC.FindTopLevelGameObject("Player").GetComponent<PlayerCustom>();
            player.InitDBCharacterData();
        },
        (error) => loginInfoTxt.text = "Login Failed");
    }

    // 회원가입
    public void Register()
    {
        var request = new RegisterPlayFabUserRequest
        {
            Email = signIdInput.text,
            Password = signPassWordInput.text,
            DisplayName = signNickNameInput.text,
            Username = signNickNameInput.text
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, (result) =>
        {
            signInInfoTxt.text = "Sign In";
            signUI.SetActive(false);
        },
        (error) => Debug.Log(error));
    }

    // 게임 끄기
    public void OnGameQuit()
    {
        Application.Quit();
    }

    // 가입 취소
    public void QuitRegister()
    {
        signUI.SetActive(false);
    }

    // 가입 버튼
    public void OnRegister()
    {
        signUI.SetActive(true);
    }
}
