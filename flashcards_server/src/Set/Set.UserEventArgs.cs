using System;

namespace flashcards_server.Set
{
	public sealed partial class Set
	{
		public class UserEventArgs : EventArgs
		{
			public uint user { get; set;}
		}
	}

}
