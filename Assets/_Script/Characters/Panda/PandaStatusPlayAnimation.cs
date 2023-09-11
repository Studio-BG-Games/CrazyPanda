using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandaStatusPlayAnimation : MonoBehaviour
{
    // обновляем статус текущей проигрываемойанимации из эфентов самой анимации (так же обновляются при кликах из CharacterMovementControl)
    public void UpdateAnimationStatus(string nameCurrentAnimation)
    {
        _PandaStatus.pandaAnimationPlay = nameCurrentAnimation;
    }
}
