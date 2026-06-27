using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System.Linq; // Listeleri filtrelemek için

public class EnemyAI : MonoBehaviour
{
    [Header("AI Settings")]
    [SerializeField, Tooltip("Yapay zeka kaç saniyede bir hamle düşünsün?")] 
    private float _thinkInterval = 3f;

    private WaitForSeconds _cachedThinkDelay;

    private void Start()
    {
        _cachedThinkDelay = new WaitForSeconds(_thinkInterval);
        StartCoroutine(AILoop());
    }

    private IEnumerator AILoop()
    {
        // Oyun çalıştığı sürece dönen zeka döngüsü
        while (true)
        {
            yield return _cachedThinkDelay;
            MakeDecision();
        }
    }

    private void MakeDecision()
    {
        // 1. Sahnedeki tüm kuleleri bul (Playable projelerinde çok az kule olduğu için FindObjectsOfType kabul edilebilir,
        // ama en doğrusu NodeManager üzerinden listeyi çekmektir. Şimdilik hızlı test için böyle yapıyoruz).
        BaseNode[] allNodes = FindObjectsOfType<BaseNode>();

        // 2. Kırmızı (Enemy) kuleleri bul ve içlerinde en az 10 askeri olanları filtrele
        List<BaseNode> myNodes = allNodes.Where(n => n.CurrentTeam == TeamType.Enemy && n.UnitCount > 10).ToList();

        if (myNodes.Count == 0) return; // Saldıracak gücüm yoksa pas geç

        // 3. Rastgele bir kulemi seç
        BaseNode attackerNode = myNodes[Random.Range(0, myNodes.Count)];

        // 4. Hedef bul (Benim olmayan kuleler: Player veya Neutral)
        List<BaseNode> targetNodes = allNodes.Where(n => n.CurrentTeam != TeamType.Enemy).ToList();

        if (targetNodes.Count == 0) return; // Hedef kalmadıysa (oyunu Enemy kazandıysa) dur

        // 5. Rastgele bir hedef seç
        BaseNode targetNode = targetNodes[Random.Range(0, targetNodes.Count)];

        // 6. ŞOV KISMI: Aynı InputManager'ın (oyuncunun) yaptığı gibi SİSTEME EMİR FIRLAT!
        // Sistem emri AI mı verdi, oyuncu mu verdi bilmez, sadece emri uygular.
        AttackData attackCommand = new AttackData
        {
            StartNode = attackerNode,
            TargetNode = targetNode
        };

        // Bu event'i manuel tetikliyoruz (Reflection veya doğrudan bağlı olan bir kanaldan tetikletebiliriz,
        // Ancak InputManager'daki event'i tetiklemek için InputManager'da tetikleyici bir public metod açmamız gerekebilir).
        GameEvents.OnAttackIssued?.Invoke(new AttackData { StartNode = attackerNode, TargetNode = targetNode });
        // DÜZELTME: Event'ler (Action) sadece tanımlandığı sınıfın İÇİNDEN Invoke edilebilir.
        // Bu yüzden InputManager.OnAttackIssued event'ini dışarıdan tetikleyemeyiz.
        // Çözüm: CombatManager'daki komutu AI için doğrudan çağırılabilir yapabiliriz VEYA
        // InputManager'a "SimulateAttack" diye bir public metod ekleriz.
    }
}