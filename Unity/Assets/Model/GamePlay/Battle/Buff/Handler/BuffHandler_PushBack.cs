﻿using ETModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[BuffType(BuffIdType.PushBack)]
public class BuffHandler_PushBack : BaseBuffHandler, IBuffActionWithGetInputHandler
{

    public void ActionHandle(BuffHandlerVar buffHandlerVar)
    {
        try
        {
            Buff_PushBack buff = (Buff_PushBack)buffHandlerVar.data;


            if (!buffHandlerVar.GetBufferValue(out BufferValue_Dir buffer_dir))
            {
                return;
            }
            if (!buffHandlerVar.GetBufferValue(out BufferValue_TargetUnits bufferValue_TargetUnits))
            {
                return;
            }

            foreach (var v in bufferValue_TargetUnits.targets)
            {
                if (buffHandlerVar.GetBufferValue(out BufferValue_AttackSuccess attackSuccess))
                {
                    if (!attackSuccess.successDic[v.Id]) continue;
                }
                Vector3 aimPos = v.Position + (buffer_dir.dir * buff.distance);
                PushBack(v, aimPos, buff).Coroutine();
            }
        }
        catch (Exception e)
        {
            Log.Error(e.ToString());
        }
    }

    async ETVoid PushBack(Unit unit, Vector3 target, Buff_PushBack buff)
    {

        CharacterMoveComponent characterMoveComponent = unit.GetComponent<CharacterMoveComponent>();
        float moveSpeed = Vector3.Distance(unit.Position, target) / buff.moveDuration;
        CharacterStateComponent characterStateComponent = unit.GetComponent<CharacterStateComponent>();
        characterStateComponent.Set(SpecialStateType.NotInControl, true);
        await characterMoveComponent.PushBackedTo(target, moveSpeed);
        characterStateComponent.Set(SpecialStateType.NotInControl, false);
    }

}



