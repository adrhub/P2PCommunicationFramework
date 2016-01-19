namespace P2PCommunicationLibrary.Net
{
    interface ICommunicator
    {
        byte[] Read();        
        void Write(byte[] buffer);
    }
}
