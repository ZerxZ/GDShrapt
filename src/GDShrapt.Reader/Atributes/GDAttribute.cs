﻿namespace GDShrapt.Reader
{
    public class GDAttribute : GDNode,
       ITokenOrSkipReceiver<GDAt>,
       ITokenOrSkipReceiver<GDIdentifier>,
       ITokenOrSkipReceiver<GDOpenBracket>,
       ITokenOrSkipReceiver<GDDataParametersList>,
       ITokenOrSkipReceiver<GDCloseBracket>
    {
        public GDAt At
        {
            get => _form.Token0;
            set => _form.Token0 = value;
        }

        public GDIdentifier Name
        {
            get => _form.Token1;
            set => _form.Token1 = value;
        }

        public GDOpenBracket OpenBracket
        {
            get => _form.Token2;
            set => _form.Token2 = value;
        }

        public GDDataParametersList Parameters
        {
            get => _form.Token3 ?? (_form.Token3 = new GDDataParametersList());
            set => _form.Token3 = value;
        }

        public GDCloseBracket CloseBracket
        {
            get => _form.Token4;
            set => _form.Token4 = value;
        }

        public enum State
        {
            At,
            Name,
            OpenBracket,
            Parameters,
            CloseBracket,
            Completed
        }

        readonly GDTokensForm<State, GDAt, GDIdentifier, GDOpenBracket, GDDataParametersList, GDCloseBracket> _form;
        public override GDTokensForm Form => _form;
        public GDTokensForm<State, GDAt, GDIdentifier, GDOpenBracket, GDDataParametersList, GDCloseBracket> TypedForm => _form;

        public GDAttribute()
        {
            _form = new GDTokensForm<State, GDAt, GDIdentifier, GDOpenBracket, GDDataParametersList, GDCloseBracket>(this);
        }

        public override GDNode CreateEmptyInstance()
        {
           return new GDAttribute();
        }

        internal override void Visit(IGDVisitor visitor)
        {
            visitor.Visit(this);
        }

        internal override void Left(IGDVisitor visitor)
        {
            visitor.Left(this);
        }

        internal override void HandleChar(char c, GDReadingState state)
        {
            if (_form.State != State.Completed && this.ResolveSpaceToken(c, state))
                return;

            switch (_form.State)
            {
                case State.At:
                    this.ResolveAt(c, state);
                    break;
                case State.Name:
                    this.ResolveIdentifier(c, state);
                    break;
                case State.OpenBracket:
                    this.ResolveOpenBracket(c, state);
                    break;
                case State.Parameters:
                    _form.State = State.CloseBracket;
                    state.PushAndPass(Parameters, c);
                    break;
                case State.CloseBracket:
                    this.ResolveCloseBracket(c, state);
                    break;
                default:
                    state.PopAndPass(c);
                    break;
            }
        }

        internal override void HandleNewLineChar(GDReadingState state)
        {
            state.PopAndPassNewLine();
        }

        void ITokenReceiver<GDAt>.HandleReceivedToken(GDAt token)
        {
            if (_form.IsOrLowerState(State.At))
            {
                _form.State = State.Name;
                At = token;
                return;
            }

            throw new GDInvalidStateException();
        }

        void ITokenSkipReceiver<GDAt>.HandleReceivedTokenSkip()
        {
            if (_form.IsOrLowerState(State.At))
            {
                _form.State = State.Name;
                return;
            }

            throw new GDInvalidStateException();
        }

        void ITokenReceiver<GDIdentifier>.HandleReceivedToken(GDIdentifier token)
        {
            if (_form.IsOrLowerState(State.Name))
            {
                _form.State = State.OpenBracket;
                Name = token;
                return;
            }

            throw new GDInvalidStateException();
        }

        void ITokenSkipReceiver<GDIdentifier>.HandleReceivedTokenSkip()
        {
            if (_form.IsOrLowerState(State.Name))
            {
                _form.State = State.OpenBracket;
                return;
            }

            throw new GDInvalidStateException();
        }

        void ITokenReceiver<GDOpenBracket>.HandleReceivedToken(GDOpenBracket token)
        {
            if (_form.IsOrLowerState(State.OpenBracket))
            {
                _form.State = State.Parameters;
                OpenBracket = token;
                return;
            }

            throw new GDInvalidStateException();
        }

        void ITokenSkipReceiver<GDOpenBracket>.HandleReceivedTokenSkip()
        {
            if (_form.IsOrLowerState(State.OpenBracket))
            {
                _form.State = State.Completed;
                return;
            }

            throw new GDInvalidStateException();
        }

        void ITokenReceiver<GDCloseBracket>.HandleReceivedToken(GDCloseBracket token)
        {
            if (_form.IsOrLowerState(State.CloseBracket))
            {
                _form.State = State.Completed;
                CloseBracket = token;
                return;
            }

            throw new GDInvalidStateException();
        }

        void ITokenSkipReceiver<GDCloseBracket>.HandleReceivedTokenSkip()
        {
            if (_form.IsOrLowerState(State.CloseBracket))
            {
                _form.State = State.Completed;
                return;
            }

            throw new GDInvalidStateException();
        }

        void ITokenReceiver<GDDataParametersList>.HandleReceivedToken(GDDataParametersList token)
        {
            if (_form.IsOrLowerState(State.Parameters))
            {
                _form.State = State.CloseBracket;
                Parameters = token;
                return;
            }

            throw new GDInvalidStateException();
        }

        void ITokenSkipReceiver<GDDataParametersList>.HandleReceivedTokenSkip()
        {
            if (_form.IsOrLowerState(State.Parameters))
            {
                _form.State = State.CloseBracket;
                return;
            }

            throw new GDInvalidStateException();
        }
    }
}
