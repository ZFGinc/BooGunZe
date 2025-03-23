using Photon.Pun;
using TMPro;
using UnityEngine;

public class ChatControll : MonoBehaviour, IPunObservable
{
    private PhotonView myView;
    public string chatRoomText;
    public string messageToSend;
    public TextMeshProUGUI chatText;
    public TMP_InputField lineTextPrint;
    
    public void OnPhotonSerializeView(PhotonStream streem, PhotonMessageInfo info){
        if(streem.IsWriting){
            streem.SendNext(chatText.text);
        }
        else{
            this.chatText.text = (string)streem.ReceiveNext();
        }
    }

    private void Update()
    {
        messageToSend = lineTextPrint.text;
    }
    [PunRPC]
    public void SendToChat()
    {
        chatRoomText += PhotonNetwork.NickName.ToString() +": " + messageToSend;
        chatText.text += "\n";
        chatText.text += chatRoomText;
        lineTextPrint.text = "";
    }
    
}





















