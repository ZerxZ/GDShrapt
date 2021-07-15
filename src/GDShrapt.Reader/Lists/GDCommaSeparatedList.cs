﻿namespace GDShrapt.Reader
{
    public abstract class GDCommaSeparatedList<NODE> : GDSeparatedList<NODE, GDComma>,
        ITokenReceiver<GDNewLine>, 
        INewLineReceiver,
        ITokenReceiver<GDComma>
        where NODE : GDSyntaxToken
    {
        internal abstract GDReader ResolveNode();
        internal abstract bool IsStopChar(char c);

        internal override void HandleChar(char c, GDReadingState state)
        {
            if (IsSpace(c))
            {
                ListForm.AddToEnd(state.Push(new GDSpace()));
                state.PassChar(c);
                return;
            }

            if (c == ',')
            {
                ListForm.AddToEnd(new GDComma());
                return;
            }
            else
            {
                if (!IsStopChar(c))
                {
                    state.PushAndPass(ResolveNode(), c);
                    return;
                }
            }

            state.PopAndPass(c);
        }

        internal override void HandleNewLineChar(GDReadingState state)
        {
            ListForm.AddToEnd(new GDNewLine());
        }

        void INewLineReceiver.HandleReceivedToken(GDNewLine token)
        {
            ListForm.AddToEnd(token);
        }

        void ITokenReceiver<GDNewLine>.HandleReceivedToken(GDNewLine token)
        {
            ListForm.AddToEnd(token);
        }

        void ITokenReceiver<GDComma>.HandleReceivedToken(GDComma token)
        {
            ListForm.AddToEnd(token);
        }
    }
}
