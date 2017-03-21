using System;
using Newtonsoft.Json;

namespace XOClientXamarin
{
    public class TTTPacket
    {
        public TTTPacket()
        {
            Matrix = null;
        }

        public TTTPacket(string playerTurn, string unit, int buttonNumber, string[] matrix, string gameResult)
        {
            PlayerTurn = playerTurn;
            Unit = unit;
            ButtonNumber = buttonNumber;
            Matrix = matrix;
            GameResult = gameResult;
        }

        public string PlayerTurn;
        public string Unit;
        public int ButtonNumber;
        public string[] Matrix;
        public string GameResult;

        public static string EncodeJson(TTTPacket packet)
        {
            return JsonConvert.SerializeObject(packet);
        }

        public static TTTPacket DecodeJson(string jsonString)
        {
            return JsonConvert.DeserializeObject<TTTPacket>(jsonString);
        }
    }
}

