using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace WhiteLabel.ViewModels.Onboarding
{
    public class InstructionsViewModel
    {
        private readonly Command _closeCommand;
        private readonly Command _moveNextCommand;
        private readonly Func<Task> _closeAction;
        private readonly Func<Task> _moveNextAction;
        private bool _closing;
        private bool _movingNext;

        public InstructionsViewModel(Func<Task> closeAction, Func<Task> moveNextAction)
        {
            _closeAction = closeAction;
            _moveNextAction = moveNextAction;

            _closeCommand = new Command(async () => await Close(), () => !Closing);
            _moveNextCommand = new Command(async () => await MoveNext(), () => !MovingNext);
        }

        public bool Closing
        {
            get { return _closing; }

            set
            {
                if (_closing != value)
                {
                    _closing = value;
                    _closeCommand.ChangeCanExecute();
                }
            }
        }

        public bool MovingNext
        {
            get { return _movingNext; }

            set
            {
                if (_movingNext != value)
                {
                    _movingNext = value;
                    _moveNextCommand.ChangeCanExecute();
                }
            }
        }

        public ICommand CloseCommand => _closeCommand;

        public ICommand MoveNextCommand => _moveNextCommand;

        private async Task Close()
        {
            if (!Closing)
            {
                Closing = true;

                try
                {
                    await _closeAction();
                }
                finally
                {
                    Closing = false;
                }
            }
        }

        private async Task MoveNext()
        {
            if (!MovingNext)
            {
                MovingNext = true;

                try
                {
                    await _moveNextAction();
                }
                finally
                {
                    MovingNext = false;
                }
            }
        }
    }
}
