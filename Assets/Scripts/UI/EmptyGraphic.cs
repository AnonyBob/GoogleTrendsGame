using UnityEngine.UI;

namespace GoogleTrends.UI
{
    public class EmptyGraphic : Graphic
    {
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear ();
        }
    }
}