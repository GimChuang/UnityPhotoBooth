using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_PhotoBooth_Web : MonoBehaviour {

    // This script plays a game manager role in the game.

    // Singleton
    public static Test_PhotoBooth_Web Instance { get; private set; }

    // GameStates
    public GameStateController gameStateController = new GameStateController();
    public ChooseBGState chooseBGState;
    public TakePhotoState takePhotoState;
    public ConfirmState confirmState;
    public ResultState resultState;
    public float timeToGoBackToMainMenu = 30f;

    Extension.GameState prevGameState;

    // Time to take photo is limited  
    public int limit_takePhoto = 3;
    int time_TakePhoto;
    public bool CanTakePhoto { get { return canTakePhoto; } }
    bool canTakePhoto;

    // Tools and Controllers
    public WebcamTool webcamTool;
    public PhotoTool photoTool;
    public PhotoAnimController photoAnimController;
    public WebTool webTool;
    public QRCodeGenerator qrCodeGenerator;
    public UIController uiController;

    // Saving Settings
    public string photoFileNamePrefix = "Photo";
    public string photoSavePath;

    public string qrFileNamePrefix = "QR";
    public string qrSavePath;

    bool isGameReady;

    #region SOME_PROPERTIES

    public int PlayerCount { get { return playerCount; } }
    int playerCount;

    public Texture2D Tex_takenPhoto { get { return tex_takenPhoto; } set { tex_takenPhoto = value; } }
    Texture2D tex_takenPhoto;

    public string PhotoFileName { get { return photoFileName; } }
    string photoFileName;

    public Texture2D Tex_qrCode { get { return tex_qrCode; } set { tex_qrCode = value; } }
    Texture2D tex_qrCode;

    #endregion SOME_PROPERTIES

    #region CANVAS_KEYING

    [Header("Keying Canvas")]
    public GameObject canvas_keying;
    public KeyCode key_canvas_keying = KeyCode.D;

    #endregion CANVAS_KEYING

    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

#if UNITY_EDITOR
        //Debug.unityLogger.logEnabled = true;
#elif UNITY_STANDALONE
        //Debug.unityLogger.logEnabled = false;
#endif

    }

    void OnEnable()
    {
        PhotoAnimController.OnCountDownFinish += HandleCountDownFinish;
        PhotoTool.OnPhotoTaken += HandlePhotoTaken;
        WebTool.OnUploadPhotoReqFinish += HandleUploadReqFinish;
        QRCodeGenerator.OnQRCodeGenerated += HandleQRCodeGenerated;
    }

    void OnDisable()
    {
        PhotoAnimController.OnCountDownFinish -= HandleCountDownFinish;
        PhotoTool.OnPhotoTaken -= HandlePhotoTaken;
        WebTool.OnUploadPhotoReqFinish -= HandleUploadReqFinish;
        QRCodeGenerator.OnQRCodeGenerated -= HandleQRCodeGenerated;
    }

    void Start () {

        // Create States
        chooseBGState = new ChooseBGState(gameStateController, timeToGoBackToMainMenu);
        takePhotoState = new TakePhotoState(gameStateController, timeToGoBackToMainMenu);
        confirmState = new ConfirmState(gameStateController, timeToGoBackToMainMenu);
        resultState = new ResultState(gameStateController, timeToGoBackToMainMenu);


        // Controllers
        webcamTool.Init();
        photoTool.Init();
        photoAnimController.Init();
        uiController.Init();

        ResetGame();

        // Set the first gameState
        GoToState(Extension.GameState.ChooseBGState);

        isGameReady = true;
    }
	
	void Update () {

        if(isGameReady)
            gameStateController.StateUpdate();

        if (Input.GetKeyDown(key_canvas_keying))
            canvas_keying.SetActive(!canvas_keying.activeInHierarchy);
    }

    #region GAMESTATES

    public void GoToState(Extension.GameState _state)
    {
        switch (_state)
        {
            case Extension.GameState.ChooseBGState:
                prevGameState = Extension.GameState.Non;
                gameStateController.SetState(chooseBGState);
                break;
            case Extension.GameState.TakePhotoState:
                prevGameState = Extension.GameState.ChooseBGState;
                gameStateController.SetState(takePhotoState);
                break;
            case Extension.GameState.ConfirmState:
                prevGameState = Extension.GameState.TakePhotoState;
                gameStateController.SetState(confirmState);
                break;
            case Extension.GameState.ResultState:
                prevGameState = Extension.GameState.ConfirmState;
                gameStateController.SetState(resultState);
                break;
        }
    }

    // TODO find better way to change between states

    public void GoToPrevState()
    {
        GoToState(prevGameState);
    }

    public void GoToNextState()
    {
        switch (gameStateController.State.GameState)
        {
            case Extension.GameState.ChooseBGState:
                gameStateController.SetState(takePhotoState);
                break;
            case Extension.GameState.TakePhotoState:
                gameStateController.SetState(confirmState);
                break;
            case Extension.GameState.ConfirmState:
                gameStateController.SetState(resultState);
                break;
            case Extension.GameState.ResultState:
                gameStateController.SetState(chooseBGState);
                break;
        }
    }
    
    #endregion GAMESTATES

    public void HandleBtnTakePhotoClicked()
    {
        // Hide the button
        uiController.ActivateBtnTakePhoto(false);
        // Play count down animation
        photoAnimController.PlayCountDownAnim();
    }

    void HandleCountDownFinish()
    {
        Debug.Log("Count Down Finish!");
        photoTool.TakePhoto();
    }

    void HandleShotFinish()
    {
        Debug.Log("Shot Finish!");
    }

    void HandlePhotoTaken(Texture2D _takenPhoto)
    {

        photoAnimController.PlayShotAnim();

        tex_takenPhoto = _takenPhoto;
        // Show rImg_takenPhoto
        uiController.ApplyTakenPhotoToRawImage(_takenPhoto);

        time_TakePhoto++;
        if (time_TakePhoto >= limit_takePhoto)
            canTakePhoto = false;

        GoToState(Extension.GameState.ConfirmState);
    }

    public void HandleBtnConfirmClicked()
    {
        // Show the loading icon
        uiController.ActivateBtnPrevAndNext_ConfirmState(false);   
        uiController.ActivateUploadingProcess(true);

        /*
        // Save taken photo as a png file
        photoFileName = FileIOUtility.GenerateFileName(fileNamePrefix, playerCount, FileIOUtility.FileExtension.PNG);
        FileIOUtility.SaveImage(tex_takenPhoto, savePath, photoFileName, FileIOUtility.FileExtension.PNG);
        */

        webTool.UploadPhoto(tex_takenPhoto, photoFileName);
    }

    void HandleUploadReqFinish(bool _success, string _photoUrl)
    {
        if (_success)
        {
            if (_photoUrl != null)
            {
                // Generate QR Code
                qrCodeGenerator.GenerateQRCode(_photoUrl);
            }
            else
                Debug.LogError("photoURL == null");
        }
        else
        {
            isGameReady = false;
            // Show Error Canvas
            uiController.ActivateCanvasNetworkError(true);
        }
    }

    void HandleQRCodeGenerated(Texture2D _generatedQRCode)
    {
        tex_qrCode = _generatedQRCode;
        uiController.SetQRCode(tex_qrCode);

        playerCount++;

        Debug.Log("PlayerCount: " + playerCount);
        /*
        gameData.playerCount = playerCount;
        bool saveGameDataSuccess = false;
        dataManager.SaveGameData(gameData, ref saveGameDataSuccess);
        if (saveGameDataSuccess == false)
        {
            isGameReady = false;
            uiController.ActivateErrorCanvas(true, Extension.ErrorType.Save);
            //uiController.ActivateErrorCanvas(true,
            //"Something's wrong \n when saving GameData.");
        }*/

        // Save taken photo as a png file
        photoFileName = FileIOUtility.GenerateFileName(photoFileNamePrefix, playerCount, FileIOUtility.FileExtension.PNG);
        FileIOUtility.SaveImage(tex_takenPhoto, photoSavePath, photoFileName, FileIOUtility.FileExtension.PNG);

        // Save QRCode
        string qrFileName = FileIOUtility.GenerateFileName(qrFileNamePrefix, playerCount, FileIOUtility.FileExtension.PNG);
        FileIOUtility.SaveImage(tex_qrCode, qrSavePath, qrFileName, FileIOUtility.FileExtension.PNG);

        // Disable the loading icon
        uiController.ActivateUploadingProcess(false);

        // Go to the Result state
        GoToState(Extension.GameState.ResultState);
    }

    public void ResetGame()
    {
        canTakePhoto = true;
        time_TakePhoto = 0;
    }
}
