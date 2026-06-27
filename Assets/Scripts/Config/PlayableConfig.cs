using UnityEngine;

namespace Config
{	
	[CreateAssetMenu(fileName = "PlayableConfig", menuName = "PlayableAd/Config Panel")]
	public class PlayableConfig : ScriptableObject
	{
		[Header("GAMEPLAY SETTINGS")]
		[Range(0.1f, 1f), Tooltip("Kuleye tıklandığında askerlerin yüzde kaçı yola çıkacak?")] 
		public float SendRatio = 0.5f;

		[Range(0.01f, 0.5f), Tooltip("Askerlerin kulelerden fırlama hızı (Aralığı)")] 
		public float SpawnDelay = 0.05f;

		[Range(1f, 10f), Tooltip("Askerlerin yoldaki ilerleme hızı")]
		public float UnitMoveSpeed = 4f;

		[Header("VISUAL & COLORS")]
		public Color NeutralColor = Color.gray;
		public Color PlayerColor = Color.blue;
		public Color EnemyColor = Color.red;

		[Header("ANIMATIONS")]
		[Range(1f, 1.5f), Tooltip("Asker üretildiğinde kulenin şişme oranı (Kalp Atışı Gücü)")]
		public float HeartbeatPulseAmount = 1.15f;

		[Range(0.5f, 1f), Tooltip("Kule hasar aldığında içeri büzüşme oranı")]
		public float DamageShrinkAmount = 0.8f;

		[Range(1f, 20f), Tooltip("Animasyonların yaylanma/esneme hızı")]
		public float ScaleLerpSpeed = 10f;

		[Range(1f, 20f), Tooltip("Renk geçişlerinin yumuşaklık hızı")]
		public float ColorLerpSpeed = 5f;
	}
}