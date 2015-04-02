using System.Collections.ObjectModel;

namespace Hashcode2015.TestRound.App.Model
{
	public class NodeList<T> : Collection<Node<T>>
	{
		public NodeList() { }

		public NodeList(int initialSize)
		{
			// Add the specified number of items
			for (int i = 0; i < initialSize; i++)
				base.Items.Add(default(Node<T>));
		}

		public Node<T> FindByValue(T value)
		{
			// search the list for the value
			foreach (Node<T> node in Items)
				if (node.Value.Equals(value))
					return node;

			// if we reached here, we didn't find a matching node
			return null;
		}
	}
}
