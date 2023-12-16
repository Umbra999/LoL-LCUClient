namespace Hexed.LCU
{
    public class OnWebsocketEventArgs : EventArgs
    { 
        public string Path { get; set; }  
        public string Type { get; set; }
        public object Data { get; set; }
    }
}
