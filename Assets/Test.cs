using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;

public class Test : MonoBehaviour {

	public GameObject LoggedInUI;
	public GameObject NotLoggedInUI;

	void Awake() {

		if (!FB.IsInitialized) {
			FB.Init (InitCallBack);
		}

		ShowUI ();
	}

	void InitCallBack() {
		Debug.Log("FB has been initialized.");
	}


	public void Login() {
		if (!FB.IsLoggedIn) {
			FB.LogInWithReadPermissions (new List<string> {"user_friends"}, LoginCallBack);
		}
	}

	void LoginCallBack (ILoginResult result) {
		if (result.Error == null) {
			Debug.Log ("FB has logged in.");
			ShowUI ();
		} else {
			Debug.Log ("Error during login:" + result.Error);
		}
	}

	void ShowUI() {
		if (FB.IsLoggedIn) {
			LoggedInUI.SetActive (true);
			NotLoggedInUI.SetActive (false);	

			FB.API ("me/picture?width=100&height=100", HttpMethod.GET, PictureCallBack);
			FB.API ("me?fields=first_name", HttpMethod.GET, NameCallBack);

		} else {
			LoggedInUI.SetActive (false);
			NotLoggedInUI.SetActive (true);
		}
	}

	void PictureCallBack(IGraphResult result) {
		Texture2D image = result.Texture;
		LoggedInUI.transform.FindChild ("ProfilePicture").GetComponent<Image> ().sprite = Sprite.Create (image, new Rect (0, 0, 100, 100), new Vector2 (0.5f, 0.5f));
	}

	void NameCallBack(IGraphResult result) {
//		Debug.Log (result.RawResult);
		IDictionary<string, object> profile = result.ResultDictionary;
		LoggedInUI.transform.FindChild ("Name").GetComponent<Text> ().text = "Hello " + profile ["first_name"];
	}

	public void Logout() {
		FB.LogOut ();
		ShowUI ();
	}
}
