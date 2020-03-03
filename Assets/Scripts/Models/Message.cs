using System.Collections;
using System.Collections.Generic;

public class Message 
{
    public string messageText;
    public string hexColor;

    public Message(string text, string color="#ffffff") {
        messageText = text;
        hexColor = color;
    }
}
