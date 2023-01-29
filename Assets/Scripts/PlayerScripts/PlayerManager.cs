using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace GameManager
{
    public class PlayerManager : MonoBehaviour, IHittable
    {
        public static PlayerManager pm;
        Animator anim;
        FPPlayerLocomotion playerLoco;
        private AudioManager _am;

        [Header("Player Health / Stamina / Mana")]
        [SerializeField] public float _curHealth;
        [SerializeField] public float _maxHealth = 10f;
        [SerializeField] public float _curStamina;
        [SerializeField] public float _maxStamina = 100f;
        [SerializeField] public float _staminaRegenTime = 4f;
        [SerializeField] public float _curMana;
        [SerializeField] public float _maxMana = 100f;

        [SerializeField] public int skillPoints = 0;

        [Header("Consumable Settings")]
        [SerializeField] private int _healthPotionAmount;
        [SerializeField] private int _manaPotionAmount;
        [SerializeField] private int _buffPotionAmount;

        [SerializeField] private float _healthPotionHeal = 40f;
        [SerializeField] private float _manaPotionHeal = 40f;

        [Header("Collectables")]
        [SerializeField] public int meatAmount = 0;
        [SerializeField] public int goldAmount = 0;

        [Header("Audio")]
        public AudioClip goldPickup, expPickup, bottlePickup;

        private bool isDead = false;
        public bool usingItem = false;
        private float time;
        private float placeholder;
        public bool gameplayPaused;

        public float MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }
        public float CurrentHealth { get { return _curHealth; } set { _curHealth = value; } }

        public float MaxStamina { get { return _maxStamina; } set { _maxStamina = value; } }
        public float CurrentStamina { get { return _curStamina; } set { _curStamina = value; } }
        public float StaminaRegenTime { get { return _staminaRegenTime; } set { _staminaRegenTime = value; } }

        public float MaxMana { get { return _maxMana; } set { _maxMana = value; } }
        public float CurrentMana { get { return _curMana; } set { _curMana = value; } }

        public float HealthPotionHeal { get { return _healthPotionHeal; } set { _healthPotionHeal = value; } }
        public float ManaPotionHeal { get { return _manaPotionHeal; } set { _manaPotionHeal = value; } }

        public int HealthPotionAmount { get { return _healthPotionAmount; } set { _healthPotionAmount = value; } }
        public int ManaPotionAmount { get { return _manaPotionAmount; } set { _manaPotionAmount = value; } }
        public int BuffPotionAmount { get { return _buffPotionAmount; } set { _buffPotionAmount = value; } }
        public int MonsterMeatAmount { get { return meatAmount; } set { meatAmount = value; } }

        private void Awake()
        {
            anim = GetComponentInChildren<Animator>();
            playerLoco = GetComponent<FPPlayerLocomotion>();

            if (PlayerManager.pm == null)
            {
                PlayerManager.pm = this;
            }
            else if (PlayerManager.pm != this)
            {
                Destroy(this);
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            _am = AudioManager.instance;
            Debug.Log($"Starting ManaPotionAmount: {_manaPotionAmount}");
            //Sets current health and stamina to max
            _curHealth = _maxHealth;
            _curStamina = _maxStamina;
            _curMana = _maxMana;

            if(PlayerPrefs.GetString("Save") == "true")
            {
                LoadPlayerManager();
            }

            gameplayPaused = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (CurrentHealth <= 0)
            {
                Die();
            }

            //Regens Stamina if taken stamina damage
            if (CurrentStamina != MaxStamina)
            {
                if (placeholder == 0)
                {
                    placeholder = CurrentStamina;
                }

                if (placeholder > CurrentStamina)
                {
                    time = 0;
                    placeholder = CurrentStamina;
                }

                time += Time.deltaTime;

                if (time >= StaminaRegenTime)
                {
                    CurrentStamina = MaxStamina;
                    time = 0;
                }

                //Debug.Log(time); Disabled by Patrick temporarily
            }
        }

        /// <summary>
        /// Used to detect pickups from mob drops, will be added to as we have more to pickup :D
        /// Items dropped MUST be on the Resource layer in order to be detected.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Resource"))
            {
                var resource = other.gameObject.GetComponent<Resources>();

                if (resource != null)
                {
                    switch (resource.ResourceData.ResourceEnum)
                    {
                        case ResourceTypeEnum.MonsterMeat:
                            meatAmount += resource.ResourceData.GetAmount();
                            resource.PickupResource();
                            break;
                        case ResourceTypeEnum.Gold:
                            goldAmount += resource.ResourceData.GetAmount();
                            resource.PickupResource();
                            _am.PlaySFX(goldPickup);
                            break;
                        case ResourceTypeEnum.Health:
                            _healthPotionAmount += resource.ResourceData.GetAmount();
                            resource.PickupResource();
                            _am.PlaySFX(bottlePickup);
                            break;
                        case ResourceTypeEnum.Mana:
                            _manaPotionAmount += resource.ResourceData.GetAmount();
                            resource.PickupResource();
                            _am.PlaySFX(bottlePickup);
                            break;
                    }
                }
            }
        }

        public void GetHit(int damage)
        {
            if (isDead == false)                                               // If player is not dead,
            {
                CurrentHealth -= damage;                                     // Decrease health by 1

                if (CurrentHealth <= 0)                                      // Check for health less than or equal to 0
                {
                    isDead = true;                                         // dead bool = true
                }
            }
        }


        public void Die()
        {
            anim.SetTrigger("isDead");
            playerLoco.canMove = false;
            Collider collider = GetComponent<Collider>();
            collider.enabled = false;

            StartCoroutine(DestroyCoroutine());
        }

        IEnumerator DestroyCoroutine()
        {
            yield return new WaitForSeconds(2f);
            SceneManager.LoadSceneAsync(0);
        }

        public void GetStunned(float length)
        {
            return;
        }

        public void EnableCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void DisableCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void SavePlayerManager()
        {
            PlayerSaving.StatsSaveLoad.Save(GameManager.PlayerManager.pm);
        }


        public void LoadPlayerManager()
        {
            PlayerSaving.SaveToken.PlayerSaveToken data = PlayerSaving.StatsSaveLoad.Load();

            if(data != null)
            {
                pm.GetComponent<LevelSystem>().level = data.Level;
                pm.GetComponent<LevelSystem>().experience = data.EXP;
                pm.GetComponent<LevelSystem>().experienceToNextLevel = data.MaxEXP;

                pm.CurrentHealth = data.CurrentHealth;
                pm.MaxHealth = data.MaxHealth;
                pm.CurrentStamina = data.CurrentStamina;
                pm.MaxStamina = data.MaxStamina;
                pm.CurrentMana = data.CurrentMana;
                pm.MaxMana = data.MaxMana;

                pm.skillPoints = data.SkillPoints;

                pm.HealthPotionAmount = data.HealthPotionAmount;
                pm.ManaPotionAmount = data.ManaPotionAmount;
                pm.BuffPotionAmount = data.BuffPotionAmount;

                pm.HealthPotionHeal = data.HealthPotionHeal;
                pm.ManaPotionHeal = data.ManaPotionHeal;

                pm.meatAmount = data.MeatAmount;
                pm.goldAmount = data.GoldAmount;
            }
        }
    }

}
