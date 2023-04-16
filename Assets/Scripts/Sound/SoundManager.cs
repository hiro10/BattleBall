using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{ // ����炷
    // AudioSource:: �X�s�[�J�[
    // AudioClip:: CD�i�f�ށj

    //BGM
    [SerializeField] AudioSource audioSourceBGM;
    [SerializeField] AudioClip[] audioClipsBGM;

    //SE
    [SerializeField] AudioSource audioSourceSE;
    [SerializeField] AudioClip[] audioClipsSE;

    private const float BGM_VOLUME_DEFULT = 1.0f;
    private const float SE_VOLUME_DEFULT = 1.0f;

    /// <summary>
    /// BGM�̗񋓌^
    /// </summary>
    public enum BGM
    {
        Title, // �^�C�g����ʗp�̋�
        Main   // �Q�[����ʗp�̋�
    }

    /// <summary>
    /// BGM�̗񋓌^
    /// </summary>
    public enum SE
    {
        HitPaddle, // �{�[�����j�􂷂�Ƃ�
        HitWall,   // �{�[���ɐG�ꂽ��
        Explosion, // ���e����������Ƃ�
        Decision, // �{�^���̌��艹
        Close, // �{�^���̕��鉹
        CountDownSe,// �J�E���g�_�E���̉�
        CountZero,// �J�E���g0�̉�
    }

    // �V���O���g���ɂ���
    // �Q�[�����ɂ�����̂����̂��́i�V�[�����ς���Ă��j�󂳂�Ȃ��j
    // �ǂ̃R�[�h������A�N�Z�X���₷��
    public static SoundManager instance;
    /// <summary>
    ///  �V���O���g���̂���
    /// </summary>
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        // ���ʃf�[�^�̓ǂݍ��݁i����̏ꍇ�͂P������j
        audioSourceBGM.volume = PlayerPrefs.GetFloat("BGM_VOLUME", BGM_VOLUME_DEFULT);
        audioSourceSE.volume = PlayerPrefs.GetFloat("SE_VOLUME", SE_VOLUME_DEFULT);
    }

    /// <summary>
    /// BGM�̍Đ�
    /// </summary>
    public void PlayBGM(BGM bgm)
    {
        // �񋓌^���痬������BGM��I�ԁiint�ŃL���X�g�j
        audioSourceBGM.clip = audioClipsBGM[(int)bgm];
        audioSourceBGM.Play();

    }

    public void StopBgm()
    {
        audioSourceBGM.Stop();
    }

    /// <summary>
    /// SE�̍Đ�
    /// </summary>
    /// <param name="se"></param>
    public void PlaySE(SE se)
    {
        audioSourceSE.PlayOneShot(audioClipsSE[(int)se]);

    }

    /// <summary>
    /// BGM�̉��ʕύX
    /// </summary>
    /// <param name="BGMVolume"> �X���C�_�[�̃{�����[�� </param>
    public void ChangeVolumeBGM(float BGMVolume)
    {
        audioSourceBGM.volume = BGMVolume;

        // ���ʂ̕ۑ�
        PlayerPrefs.SetFloat("BGM_VOLUME", audioSourceBGM.volume);

    }

    /// <summary>
    /// SE�̉��ʕύX
    /// </summary>
    /// <param name="SEVolume"> �X���C�_�[�̃{�����[�� </param>
    public void ChangeVolumeSE(float SEVolume)
    {
        audioSourceSE.volume = SEVolume;

        // ���ʂ̕ۑ�
        PlayerPrefs.SetFloat("SE_VOLUME", audioSourceSE.volume);

    }
}
