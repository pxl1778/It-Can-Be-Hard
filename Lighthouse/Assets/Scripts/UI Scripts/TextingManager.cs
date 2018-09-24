using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextingManager : MonoBehaviour {

    [SerializeField]
    private RawImage phoneImage;
    [SerializeField]
    private RectTransform content;
    [SerializeField]
    private GameObject rightMessage;
    [SerializeField]
    private GameObject leftMessage;
    [SerializeField]
    private GameObject dateMessage;
    [SerializeField]
    private Button sendButton;
    [SerializeField]
    private AudioSource getMessageSound;
    [SerializeField]
    private AudioSource sendMessageSound;

    private bool isLeft = true;
    private string[] textConvo;
    private int currentText = 0;
    private bool canReply = false;
    private float cElapsedTime = 0;
    private float elapsedTime = 0;
    private float replyTime = 2.5f;
    private float closeTime = 3.5f;
    private bool closing = false;


    // Use this for initialization
    void Start () {
        GameManager.instance.EventMan.uiFaded.AddListener(startLerp);
        GameObject dateText = Object.Instantiate(dateMessage);
        dateText.transform.parent = content;
        dateText.GetComponentInChildren<Text>().text = GameManager.instance.DialogueMan.getLine("texting1_date");
        dateText.transform.localScale = new Vector3(1, 1, 1);
        textConvo = new string[]{ "texting1_1_1", "texting1_2_2", "texting1_3_1", "texting1_4_2", "texting1_5_2", "texting1_6_1", "texting1_7_2"};
	}
	
	// Update is called once per frame
	void Update () {
        if (!canReply && currentText < textConvo.Length && !closing)
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime >= replyTime)
            {
                GameObject newText = Object.Instantiate(leftMessage);
                newText.transform.parent = content;
                newText.transform.localScale = new Vector3(1, 1, 1);
                newText.GetComponentInChildren<Text>().text = GameManager.instance.DialogueMan.getLine(textConvo[currentText]);
                getMessageSound.PlayOneShot(getMessageSound.clip);
                StartCoroutine(LerpTextUp(0.2f, newText));
                currentText++;
                elapsedTime = 0;
                if (currentText >= textConvo.Length)
                {
                    canReply = false;
                    closing = true;
                    sendButton.interactable = false;
                    sendButton.image.color = new Color(0.51f, 0.51f, 0.51f, 1.0f);
                    return;
                }
                if (textConvo[currentText].Substring(textConvo[currentText].Length - 1, 1) == "1")
                {
                    canReply = false;
                    sendButton.interactable = false;
                    sendButton.image.color = new Color(0.51f, 0.51f, 0.51f, 1.0f);
                }
                else
                {
                    canReply = true;
                    sendButton.interactable = true;
                    sendButton.image.color = new Color(1f, 1f, 1f, 1.0f);
                }
            }
        }
        if(currentText >= textConvo.Length && closing)
        {
            Debug.Log("we made it here");
            elapsedTime += Time.deltaTime;
            if(elapsedTime >= closeTime)
            {
                Debug.Log("now here!");
                elapsedTime = 0;
                StartCoroutine(LerpPhoneDown(0.2f, -1500));
                closing = false;
            }
        }
	}

    private void OnDestroy()
    {
        GameManager.instance.EventMan.uiFaded.RemoveListener(startLerp);
    }

    public void startLerp()
    {
        StartCoroutine(LerpPhone(0.5f, 0));
    }

    public void clickReply()
    {
        if(currentText >= textConvo.Length || !canReply)
        {
            return;
        }
        
        GameObject newText = Object.Instantiate(rightMessage);
        newText.transform.parent = content;
        newText.transform.localScale = new Vector3(1, 1, 1);
        newText.GetComponentInChildren<Text>().text = GameManager.instance.DialogueMan.getLine(textConvo[currentText]);
        currentText++;
        sendMessageSound.PlayOneShot(sendMessageSound.clip);
        StartCoroutine(LerpTextUp(0.2f, newText));
        if (currentText >= textConvo.Length)
        {
            canReply = false;
            closing = true;
            sendButton.interactable = false;
            sendButton.image.color = new Color(0.51f, 0.51f, 0.51f, 1.0f);
            return;
        }
        if (textConvo[currentText].Substring(textConvo[currentText].Length - 1, 1) == "1")
        {
            canReply = false;
            sendButton.interactable = false;
            sendButton.image.color = new Color(0.51f, 0.51f, 0.51f, 1.0f);
        }
        else
        {
            canReply = true;
            sendButton.interactable = true;
            sendButton.image.color = new Color(1f, 1f, 1f, 1.0f);
        }
    }

    IEnumerator LerpPhone(float duration, float endY)
    {
        cElapsedTime = 0;
        float originalY = phoneImage.rectTransform.localPosition.y;
        while (cElapsedTime < duration)
        {
            cElapsedTime += Time.deltaTime;
            phoneImage.rectTransform.localPosition = new Vector3(phoneImage.rectTransform.localPosition.x, Mathf.Lerp(originalY, endY, calcEase(cElapsedTime / duration)), phoneImage.rectTransform.localPosition.z);
            yield return new WaitForEndOfFrame();
        }
        phoneImage.rectTransform.localPosition = new Vector3(phoneImage.rectTransform.localPosition.x, endY, phoneImage.rectTransform.localPosition.z);
    }
    IEnumerator LerpPhoneDown(float duration, float endY)
    {
        Debug.Log("lerping down");
        cElapsedTime = 0;
        
        float originalY = phoneImage.rectTransform.localPosition.y;
        while (cElapsedTime < duration)
        {
            cElapsedTime += Time.deltaTime;
            phoneImage.rectTransform.localPosition = new Vector3(phoneImage.rectTransform.localPosition.x, Mathf.Lerp(originalY, endY, calcEase(cElapsedTime / duration)), phoneImage.rectTransform.localPosition.z);
            yield return new WaitForEndOfFrame();
        }
        phoneImage.rectTransform.localPosition = new Vector3(phoneImage.rectTransform.localPosition.x, endY, phoneImage.rectTransform.localPosition.z);
        GameManager.instance.StartLoadScene("Neighborhood0");
    }

    IEnumerator LerpTextUp(float duration, GameObject text)
    {
        float textTime = 0;
        text.transform.localScale = new Vector3(0, 0, 1);
        while (textTime < duration)
        {
            textTime += Time.deltaTime;
            float scale = textTime / duration;
            if(scale > 1.0f) { scale = 1.0f; }
            scale = calcEase(scale);
            text.transform.localScale = new Vector3(scale, scale, 1);
            yield return new WaitForEndOfFrame();
        }
    }

    public float calcEase(float pAlpha)
    {
        pAlpha = (pAlpha > 1.0f) ? 1.0f : pAlpha;
        //return pAlpha * pAlpha * (3.0f - 2.0f * pAlpha);
        return pAlpha * (2.0f - pAlpha);
    }
}
