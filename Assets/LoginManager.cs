using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

//1126 Mon 과제 : 회원가입 UI만들기
//1127 Tue 과제 : 로그인화면 만들기

//회원가입 폼
public struct SignUpForm
{
    public string username;
    public string password;
    public string nickname;
}

//로그인폼
public struct LogInForm
{
    public string username;
    public string password;
}

//로그인
public struct LoginResult
{
    public int result;
}

//응답
public enum ResponseType
{
    INVALID_USERNAME=0,
    INVALID_PASSWORD,
    SUCCESS
}


public class LoginManager : MonoBehaviour
{

    //public GameObject signUpPanel;
    //public GameObject signUpButton;
    //private bool active = true;

    //선생님방식
    public Image signupPanel;
    public InputField usernameIF;
    public InputField nicknameIF;
    public InputField passwordIF;
    public InputField confirmPasswordIF;


    public Image loginPanel;
    public InputField loginUsernameIF;
    public InputField loginPasswordIF;
    public Button loginConfirmButton;


    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    //// canvasGroup
    //private CanvasGroup signPanel;
    //private CanvasGroup signButton;

    void Start()
    {

        loginConfirmButton.interactable = false;

        //GameObject signUpPanel = GetComponent<GameObject>();
        //signUpPanel.SetActive(!active);


        //signPanel = GameObject.Find("Panel_SignUp").GetComponent<CanvasGroup>();
        //signPanel.alpha = 0;
        //signButton = GameObject.Find("Button_SignUp").GetComponent<CanvasGroup>();

    }

    //회원가입 버튼 이벤트
    public void OnClickSignUpButton()
    {
        signupPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    }

    //+++++ 로그인 버튼 이벤트
    public void OnClickLoginButton()
    {
        loginPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    }

    // 회원가입 확인버튼
    public void OnClickConfirmButton()
    {
        string password = passwordIF.text;
        string confirmPassword = confirmPasswordIF.text;
        string username = usernameIF.text;
        string nickname = nicknameIF.text;

        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword)
                || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(nickname))
        {
            return;
        }

        if (password.Equals(confirmPassword))
        {
            // TODO: 서버에 회원가입 정보 전송

            SignUpForm signupForm = new SignUpForm();
            signupForm.username = username;
            signupForm.password = password;
            signupForm.nickname = nickname;


            StartCoroutine(SignUp(signupForm));
        }
    }


    //+++++로그인 확인버튼
    public void OnClickLoginConfirmButton()
    {

        //인풋필드의 문자열 가져오기
        string username = loginUsernameIF.text;
        string password = loginPasswordIF.text;


        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            return;
        }


        // TODO: 서버에 로그인 정보 전송
        LogInForm loginForm = new LogInForm();
        loginForm.username = username;
        loginForm.password = password;

        loginConfirmButton.interactable = false;

        StartCoroutine(Login(loginForm));

    }

    //취소버튼 이벤트
    public void OnClickCancleButton()
    {
        signupPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(1130, 0);
    }

    //+++++로그인 취소버튼 이벤트
    public void OnClickLoginCancleButton()
    {
        loginPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(780, 0);
    }

    public void UpdateLoginInputField()
    {
        if (!string.IsNullOrEmpty(loginUsernameIF.text) && !string.IsNullOrEmpty(loginPasswordIF.text)) //둘다 채워지면
        {
            loginConfirmButton.interactable = true;
        }
        else
        {
            loginConfirmButton.interactable = false;
        }
    }


    IEnumerator SignUp(SignUpForm form)
    {
        string postData = JsonUtility.ToJson(form);
        byte[] sendData = Encoding.UTF8.GetBytes(postData);

        using (UnityWebRequest www = UnityWebRequest.Put("http://localhost:3000/users/add", postData))
        {
            www.method = "POST";
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.Send();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }

    }

    //+++++ Login 코루틴
    IEnumerator Login(LogInForm form)
    {

        
        string postData = JsonUtility.ToJson(form);
        byte[] sendData = Encoding.UTF8.GetBytes(postData);

        using (UnityWebRequest www = UnityWebRequest.Put("http://localhost:3000/users/signin", postData))
        {
            www.method = "POST";
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.Send();
            loginConfirmButton.interactable = true;
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string resultStr = www.downloadHandler.text;

                var result = JsonUtility.FromJson<LoginResult>(resultStr);

                if (result.result == 2)
                {
                    SceneManager.LoadScene("Game");
                }
                Debug.Log(www.downloadHandler.text);
            }
        }

    }




    ////SetActive 사용----------------------------
    //public void ButtonClicked()
    //{
    //    signUpPanel.SetActive(active);
    //    signUpButton.SetActive(!active);
    //    active = !active;

    //}

    ////버튼만을 위한 클릭 메소드
    //public void OKClicked()
    //{
    //    signUpButton.SetActive(!active);
    //    active = !active;
    //}

    ////-------------------------------------------
    ////canvasGroup 사용
    //public void ButtonClick()
    //{
    //    signPanel.alpha = 1;

    //    signButton.alpha = 0;
    //    signButton.blocksRaycasts = false;

    //}


}
