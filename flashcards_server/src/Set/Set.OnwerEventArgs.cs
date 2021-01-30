using System;

namespace flashcards_server.Set
{
	public partial class Set
	{
		public class OwnerEventArgs : EventArgs
		{
			public uint user { get; set;}
		}
	}

}
