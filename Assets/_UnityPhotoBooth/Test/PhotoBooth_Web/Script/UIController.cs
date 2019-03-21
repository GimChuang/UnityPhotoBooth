using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    [Header("ChooseBG State")]
    public GameObject panel_chooseBGState;
    public RawImage rImg_bg;
    public Button btn_chooseBG_left;
    public Button btn_chooseBG_right;
    public Text txt_choosenBG;
    public Button btn_nextStep_chooseBGState;

    public Texture[] bgTextures;
    int bgIndex;

    [Header("TakePhoto State")]
    public GameObject panel_takePhotoState;
    public Button btn_prevStep_takePhotoState;
    public Button btn_takePhoto;

    [Header("Confirm State")]
    public GameObject panel_confirmState;
    public RawImage rImg_takenPhoto_confirmState;
    public Button btn_prevStep_confirmState;
    public Button btn_nextStep_confirmState;
    public GameObject panel_uploading;

    [Header("Result State")]
    public GameObject panel_resultState;
    public RawImage rImg_takenPhoto_resultState;
    public RawImage rImg_qrCode;

    [Header("Canvas-NetworkError")]
    public GameObject canvas_networkError;

    public void Init()
    {
        rImg_bg.texture = bgTextures[bgIndex];
        txt_choosenBG.text = "Background " + bgIndex;

        panel_chooseBGState.SetActive(false);
        panel_takePhotoState.SetActive(false);
        panel_confirmState.SetActive(false);
        panel_resultState.SetActive(false);

        canvas_networkError.SetActive(false);
    }

    void OnEnable()
    {
        btn_chooseBG_left.onClick.AddListener(() => ChangeBGTexture(false));
        btn_chooseBG_right.onClick.AddListener(() => ChangeBGTexture(true));
        btn_nextStep_chooseBGState.onClick.AddListener(() => Test_PhotoBooth_Web.Instance.GoToState(Extension.GameState.TakePhotoState));
        btn_prevStep_takePhotoState.onClick.AddListener(() => Test_PhotoBooth_Web.Instance.GoToPrevState());
        btn_takePhoto.onClick.AddListener(() => Test_PhotoBooth_Web.Instance.HandleBtnTakePhotoClicked());
        btn_prevStep_confirmState.onClick.AddListener(() => Test_PhotoBooth_Web.Instance.GoToPrevState());
        btn_nextStep_confirmState.onClick.AddListener(() => Test_PhotoBooth_Web.Instance.HandleBtnConfirmClicked());
    }

    void OnDisable()
    {
        btn_chooseBG_left.onClick.RemoveAllListeners();
        btn_chooseBG_right.onClick.RemoveAllListeners();
        btn_nextStep_chooseBGState.onClick.RemoveAllListeners();
        btn_prevStep_takePhotoState.onClick.RemoveAllListeners();
        btn_takePhoto.onClick.RemoveAllListeners();
        btn_prevStep_confirmState.onClick.RemoveAllListeners();
        btn_nextStep_confirmState.onClick.RemoveAllListeners();
    }

    void Start () {
		
	}
	
	void Update () {
		
	}

    public void HandleStateBegin(Extension.GameState _gameState)
    {
        switch (_gameState)
        {
            case Extension.GameState.ChooseBGState:
                panel_chooseBGState.SetActive(true);
                break;
            case Extension.GameState.TakePhotoState:
                ActivateBtnTakePhoto(true);
                panel_takePhotoState.SetActive(true);
                break;
            case Extension.GameState.ConfirmState:
                ActivateBtnPrevAndNext_ConfirmState(true);
                panel_confirmState.SetActive(true);
                break;
            case Extension.GameState.ResultState:
                panel_resultState.SetActive(true);
                break;
        }
    }

    public void HandleStateEnd(Extension.GameState _gameState)
    {
        switch (_gameState)
        {
            case Extension.GameState.ChooseBGState:
                panel_chooseBGState.SetActive(false);
                break;
            case Extension.GameState.TakePhotoState:
                panel_takePhotoState.SetActive(false);
                break;
            case Extension.GameState.ConfirmState:
                panel_confirmState.SetActive(false);
                break;
            case Extension.GameState.ResultState:
                panel_resultState.SetActive(false);
                break;
        }
    }

    // Called by btn_chooseBG_left and btn_chooseBG_right
    public void ChangeBGTexture(bool _right)
    {
        if (_right)
        {
            bgIndex++;
            if (bgIndex >= bgTextures.Length) bgIndex = 0;
        }
        else
        {
            bgIndex--;
            if (bgIndex < 0) bgIndex = bgTextures.Length - 1;
        }

        rImg_bg.texture = bgTextures[bgIndex];
        txt_choosenBG.text = "Background " + bgIndex;
    }

    public void ActivateBtnTakePhoto(bool _active)
    {
        btn_prevStep_takePhotoState.gameObject.SetActive(_active);
        btn_takePhoto.gameObject.SetActive(_active);
    }

    public void ApplyTakenPhotoToRawImage(Texture2D _takenPhoto)
    {
        rImg_takenPhoto_confirmState.texture = _takenPhoto;
        rImg_takenPhoto_resultState.texture = _takenPhoto;
    }

    public void ActivateBtnPrevAndNext_ConfirmState(bool _active)
    {
        // Limit times of taking photo
        btn_prevStep_confirmState.interactable = Test_PhotoBooth_Web.Instance.CanTakePhoto ? true : false;

        btn_prevStep_confirmState.gameObject.SetActive(_active);
        btn_nextStep_confirmState.gameObject.SetActive(_active);
    }

    public void ActivateUploadingProcess(bool _active)
    {
        panel_uploading.SetActive(_active);
    }

    public void SetQRCode(Texture2D _generatedQRCode)
    {
        rImg_qrCode.texture = _generatedQRCode;
    }

    public void ActivateCanvasNetworkError(bool _active)
    {
        canvas_networkError.SetActive(_active);
    }
}
