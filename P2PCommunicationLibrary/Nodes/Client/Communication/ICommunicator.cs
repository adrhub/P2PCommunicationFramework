namespace P2PCommunicationLibrary
{
    interface ICommunicator
    {
        byte[] Read();        
        void Write(byte[] buffer);
    }
}
