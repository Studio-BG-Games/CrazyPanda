using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemStatusPlayAnimation : MonoBehaviour
{
    // обновляем статус текущей проигрываемойанимации из эфентов самой анимации (так же обновляются при кликах из CharacterMovementControl)
    public void UpdateAnimationStatusGolem(string nameCurrentAnimation)
    {
        _GolemStatus.golemAnimationPlay = nameCurrentAnimation;
    }
}
