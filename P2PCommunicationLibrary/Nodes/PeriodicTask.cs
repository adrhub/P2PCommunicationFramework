using System;
using System.Threading;
using System.Threading.Tasks;

namespace P2PCommunicationLibrary
{
    public class PeriodicTask
    {       
        private readonly Action _action;
        private readonly TimeSpan _dueTime;
        private readonly TimeSpan _interval;
        private readonly CancellationToken _token;

        public PeriodicTask(Action action, TimeSpan dueTime, TimeSpan interval, CancellationToken token)
        {           
            _action = action;
            _dueTime = dueTime;
            _interval = interval;
            _token = token;
        }

        public async Task DoPeriodicWorkAsync()
        {
            // Initial wait time before we begin the periodic loop.
            if (_dueTime > TimeSpan.Zero)
                await Task.Delay(_dueTime, _token);

            // Repeat this loop until cancelled.
            while (!_token.IsCancellationRequested)
            {
                _action();

                // Wait to repeat again.
                if (_interval > TimeSpan.Zero)
                    await Task.Delay(_interval, _token);
            }
        }
    }
}
