using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GamePlayUI : UIItem
{
    [SerializeField] private Text LevelName;
    [SerializeField] private Image HpImg;
     private float targetFillAmount; // Mức fillAmount đích của thanh HP
    private float lerpSpeed = 5f; // Tốc độ giảm fillAmount
    public void Pause()
    {
        Time.timeScale = 0;
        UIManager.Instance.OpenUI<PauseUI>();
        SoundManager.Instance.PlayClickSound();
        targetFillAmount = TankController.Instance.rateHp; // Gán giá trị ban đầu
        StartCoroutine(UpdateHp()); // Cập nhật ban đầu khi game bắt đầu
    }
    void Update()
    {
        UpdateLevelText();
        if (HpImg.type == Image.Type.Filled && targetFillAmount != TankController.Instance.rateHp)
        {
            targetFillAmount = TankController.Instance.rateHp;
            StartCoroutine(UpdateHp());
        }
    }
    public void Shoot()
    {   
        if (Time.time - TankController.Instance.lastShotTime >= TankController.Instance.timeBetweenShots)
        {
            TankController.Instance.ShootBullet();
            TankController.Instance.lastShotTime = Time.time; // Cập nhật thời gian lần bắn cuối cùng
        }
    }
    private IEnumerator UpdateHp()
    {
        // Duy trì hiệu ứng giảm dần đến giá trị mới của fillAmount
        float currentFillAmount = HpImg.fillAmount;
        float timeElapsed = 0f;

        while (timeElapsed < 1f)
        {
            HpImg.fillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, timeElapsed);
            timeElapsed += Time.deltaTime * lerpSpeed;
            yield return null;
        }

        // Đảm bảo fillAmount đạt đúng giá trị mục tiêu sau khi Coroutine kết thúc
        HpImg.fillAmount = targetFillAmount;
    }
    private void UpdateLevelText()
    {
        if (LevelName != null)
        {   
            LevelName.text = SceneManager.GetActiveScene().name; // Hiển thị với 2 chữ số, ví dụ: 01, 02
        }   
    }  
}
