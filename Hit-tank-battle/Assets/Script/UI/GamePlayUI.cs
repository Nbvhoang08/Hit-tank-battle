using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GamePlayUI : UIItem
{
    [SerializeField] private Text LevelName;
    [SerializeField] private Image HpImg;
    public void Pause()
    {
        Time.timeScale = 0;
        UIManager.Instance.OpenUI<PauseUI>();
        SoundManager.Instance.PlayClickSound();
    }
    void Update()
    {
        UpdateLevelText();
    }
    public void Shoot()
    {   
         if (Time.time - TankController.Instance.lastShotTime >= TankController.Instance.timeBetweenShots)
        {
            TankController.Instance.ShootBullet();
            TankController.Instance.lastShotTime = Time.time; // Cập nhật thời gian lần bắn cuối cùng
        }
    }

    private void UpdateLevelText()
    {
        if (LevelName != null)
        {   
            LevelName.text = SceneManager.GetActiveScene().name; // Hiển thị với 2 chữ số, ví dụ: 01, 02
        }   
    }  
}
