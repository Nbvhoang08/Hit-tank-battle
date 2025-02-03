using UnityEngine;
using System.Collections.Generic;

public class TransparentWalls : MonoBehaviour
{
     private Camera myCamera;
    public Material transparentMaterial;
    private Dictionary<Renderer, Material> wallsToRestore = new Dictionary<Renderer, Material>();  // Lưu các tường và material ban đầu

    public Transform target;  // Biến Transform để lưu đối tượng target (player)

    public void Start()
    {
        myCamera = Camera.main;
    }

    void Update()
    {
        if (target != null)
        {
            // Bắn Raycast từ camera đến target (player)
            Ray ray = new Ray(myCamera.transform.position, target.position - myCamera.transform.position);
            RaycastHit hit;

            // Tạo một danh sách mới để lưu lại các renderer bị cắt qua
            List<Renderer> wallsHit = new List<Renderer>();

            if (Physics.Raycast(ray, out hit))
            {
                // Kiểm tra nếu đối tượng va chạm là tường
                if (hit.collider.CompareTag("Wall"))
                {
                    Renderer hitRenderer = hit.collider.GetComponent<Renderer>();
                    
                    // Nếu ray hit một tường và chưa thay đổi material, thay đổi material
                    if (!wallsToRestore.ContainsKey(hitRenderer))
                    {
                        wallsToRestore.Add(hitRenderer, hitRenderer.material);  // Lưu material ban đầu
                        hitRenderer.material = transparentMaterial;  // Thay đổi material thành transparent
                    }

                    wallsHit.Add(hitRenderer);  // Thêm renderer vào danh sách các tường bị hit
                }
            }

            // Trả lại material cho các tường không bị cắt qua nữa (khi Player di chuyển)
            List<Renderer> toRestore = new List<Renderer>(wallsToRestore.Keys);
            foreach (Renderer wall in toRestore)
            {
                if (!wallsHit.Contains(wall))  // Kiểm tra nếu tường không bị hit lần nữa (Player di chuyển ra ngoài phạm vi)
                {
                    wall.material = wallsToRestore[wall];  // Trả lại material ban đầu
                    wallsToRestore.Remove(wall);  // Xóa tường đã restore khỏi danh sách
                }
            }
        }
    }

}
