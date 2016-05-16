using System.Collections.Generic;
using System.Linq;

/*
    A probability tree using weights instead of pure probabilities, allowing for easier dynamic changes
*/

public class WeightedTreeNode<T>
{
    private Dictionary<WeightedTreeNode<T>, double> children = new Dictionary<WeightedTreeNode<T>, double>();
    public WeightedTreeNode<T>[] Children { get { return children.Keys.ToArray(); } }

    public T Value;
    public WeightedTreeNode<T> Parent;

    public WeightedTreeNode(T value, WeightedTreeNode<T> parent)
    {
        this.Value = value;
        this.Parent = parent;
    }

    public WeightedTreeNode(T value) : this(value, null) { }

    public void AddChild(T value, double weight)
    {
        children.Add(
            new WeightedTreeNode<T>(value, this), 
            weight);
    }

    public WeightedTreeNode<T> GetRandomChild()
    {
        if (children.Count <= 0)
        {
            return null;
        }

        return children.Keys.ToArray()[WeightedRandomizer.GetRandomIndex(children.Values.ToArray())];
    }
}


