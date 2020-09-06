using System;
using System.Text;

namespace TauCode.Cli.Tests.Common.Hosts.Work.Mock
{
    public class MockBus : IBus
    {
        public MockWorker Worker { get; } = new MockWorker("good-worker");

        public WorkerCommandResponse Request(WorkerCommandRequest request)
        {
            try
            {
                if (request.WorkerName != this.Worker.Name)
                {
                    throw new Exception("Invalid worker name.");
                }

                var result = this.ExecuteCommand(request.Command, request.Timeout);
                var response = new WorkerCommandResponse
                {
                    Result = result,
                };

                return response;
            }
            catch (Exception ex)
            {
                var errorResponse = new WorkerCommandResponse
                {
                    Exception = ExceptionInfo.FromException(ex),
                };

                return errorResponse;
            }
        }

        private string ExecuteCommand(WorkerCommand command, int? timeout)
        {
            string result;

            switch (command)
            {
                case WorkerCommand.GetInfo:
                    result = this.GetInfo();
                    break;

                case WorkerCommand.Start:
                    this.Worker.State = MockWorkerState.Running;
                    result = this.Worker.State.ToString();
                    break;

                case WorkerCommand.Pause:
                    this.Worker.State = MockWorkerState.Paused;
                    result = this.Worker.State.ToString();
                    break;

                case WorkerCommand.Resume:
                    this.Worker.State = MockWorkerState.Running;
                    result = this.Worker.State.ToString();
                    break;

                case WorkerCommand.Stop:
                    this.Worker.State = MockWorkerState.Stopped;
                    result = this.Worker.State.ToString();
                    break;

                case WorkerCommand.Dispose:
                    this.Worker.State = MockWorkerState.Disposed;
                    result = this.Worker.State.ToString();
                    break;

                case WorkerCommand.Timeout:
                    if (timeout.HasValue)
                    {
                        this.Worker.Timeout = timeout.Value;
                    }

                    result = this.Worker.Timeout.ToString();

                    break;

                default:
                    throw new InvalidOperationException();
            }

            return result;
        }

        private string GetInfo()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Type: {this.Worker.GetType().FullName}; ");
            sb.AppendLine($"Name: {this.Worker.Name}; ");
            sb.AppendLine($"State: {this.Worker.State}");
            sb.AppendLine();

            return sb.ToString();
        }
    }
}
