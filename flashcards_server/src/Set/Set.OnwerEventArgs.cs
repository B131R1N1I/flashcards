using System;

namespace flashcards_server.Set
{
	public partial class Set
	{
		public class OwnerEventArgs : EventArgs
		{
			public User.User user { get; set;}
		}
	}

}
