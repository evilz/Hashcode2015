namespace Hashcode2015.TestRound.App.Model
{
	public class Node<T>
	{
		// Private member-variables

		public Node() { }

		public Node(T data,Node<T> parent = null, NodeList<T> neighbors = null)
		{
			Value = data;
			Parent = parent;
			Childs = neighbors ?? new NodeList<T>();
		}

		public T Value { get; private set; }

		public Node<T> Parent { get; set; } 
		public NodeList<T> Childs { get; }
	}

}
