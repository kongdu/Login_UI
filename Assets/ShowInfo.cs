using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Networking;


public class ShowInfo : MonoBehaviour {

    LoginManager loginManager;

    public Button showNickButton;
    public Text userInfoText;

    void Start() {
        loginManager = FindObjectOfType<LoginManager>();

    }

    public void OnClickShowNickButton()
    {
        
        LogInForm loginForm = new LogInForm();
        loginForm.username = loginManager.loginUsernameIF.text;
        loginForm.password = loginManager.loginPasswordIF.text;

        StartCoroutine(ShowNick(loginForm));
    }

    IEnumerator ShowNick(LogInForm form){

        //클라이언트 -> 서버로
        string postData = JsonUtility.ToJson(form);
        byte[] sendData = Encoding.UTF8.GetBytes(postData);

        using (UnityWebRequest www = UnityWebRequest.Put("http://localhost:3000/users/shownick", postData))

        // 서버 -> 클라이언트

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
                string resultStr = www.downloadHandler.text;

                Debug.Log(www.downloadHandler.text);
                userInfoText.text = resultStr;

            }
        }


    }


}
