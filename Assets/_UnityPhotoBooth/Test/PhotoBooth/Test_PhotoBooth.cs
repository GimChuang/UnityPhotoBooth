using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_PhotoBooth : MonoBehaviour {

    public WebcamTool webcamTool;
    public PhotoTool photoTool;
    public PhotoAnimController photoAnimController;

    [Header("Save Image")]
    public string savePath;
    public string fileName;
    int photoCount;

    [Header("UI")]
    public Button btn_takePhoto;
    public RawImage rImg_takenPhoto;

    [Header("Keying Canvas")]
    public GameObject canvas_keying;
    public KeyCode key_canvas_keying = KeyCode.D;

    void Start () {

        webcamTool.Init();
        photoTool.Init();
        photoAnimController.Init();

        rImg_takenPhoto.gameObject.SetActive(false);
        canvas_keying.SetActive(false);
    }

    void OnEnable()
    {
        btn_takePhoto.onClick.AddListener(HandleBtnTakePhotoClicked);
        PhotoAnimController.OnCountDownFinish += HandleCountDownFinish;
        PhotoTool.OnPhotoTaken += HandlePhotoTaken;
    }

    void OnDisable()
    {
        btn_takePhoto.onClick.RemoveAllListeners();
        PhotoAnimController.OnCountDownFinish -= HandleCountDownFinish;
        PhotoTool.OnPhotoTaken -= HandlePhotoTaken;
    }

    void Update () {


        if (Input.GetKeyDown(key_canvas_keying))
            canvas_keying.SetActive(!canvas_keying.activeInHierarchy);
            
	}

    void HandleBtnTakePhotoClicked()
    {
        // Hide the button
        btn_takePhoto.gameObject.SetActive(false);
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

        photoCount += 1;

        // Show rImg_takenPhoto
        rImg_takenPhoto.texture = _takenPhoto;
        rImg_takenPhoto.gameObject.SetActive(true);

        // Save taken photo as a png file
        string fullName = FileIOUtility.GenerateFileName(fileName, photoCount, FileIOUtility.FileExtension.PNG);
        FileIOUtility.SaveImage(_takenPhoto, savePath, fullName, FileIOUtility.FileExtension.PNG);

        // Wait for some time and go back to a playable status
        StartCoroutine(E_WaitAndReset());
    }

    IEnumerator E_WaitAndReset()
    {
        yield return new WaitForSeconds(2f);

        // Show the button again
        btn_takePhoto.gameObject.SetActive(true);

        // Hide rImg_takenPhoto
        rImg_takenPhoto.gameObject.SetActive(false);
    }

}
