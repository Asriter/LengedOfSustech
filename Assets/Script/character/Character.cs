﻿//所有角色的父类，是个抽象类
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    //属性
    protected readonly double _maxHp; //最大生命值
    protected double _hp; //当前生命
    protected int _mp = 50; //蓝量，满蓝放技能
    protected readonly double _atk; //攻击力
    protected readonly double _def; //防御
    protected List<Buff> _buffs; //rt

    //待添加
    //TODO

    //构造器
    protected Character(double hp, double atk, double def)
    {
        _maxHp = hp;
        _hp = hp;
        _atk = atk;
        _def = def;
        _buffs = new List<Buff>();
    }

    //行动，轮到角色行动时调用此方法
    public void Action()
    {
        if (_mp < 100) //怒气小于100普攻，否则skill
        {
            Attack();
            Modify_mp(25);  //普攻怒气加25
        }
        else
        {
            Skill();
            Modify_mp(0);
        }
        Check_buff_remain();  //skill后怒气归零
    }

    //调整怒气值，参数不为0时怒气加上参数，参数为0时怒气归零
    public void Modify_mp(int amount)
    {
        if (amount != 0)
        {
            _mp += amount;
            _mp = Math.Min(100, _mp); //怒气最高为100
        }
        else
            _mp = 0;
    }

    //回合结束用于减少buff持续时间，移除时间结束的buff
    private void Check_buff_remain()
    {
        for (int i = _buffs.Count - 1; i >= 0; i--)
        {
            Buff buff = _buffs[i];
            buff._remain -= 1;  //持续时间减一
            if (buff._remain <= 0)  //时间为0则移除
            {
                _buffs.Remove(buff);
            }
        }
    }

    //用于通常攻击，在攻击完成之后返回状态值，让主程序继续运行
    protected int Attack()
    {
        double atk = Count_atk();
        double damage = Count_damage(atk);
        Get_target()[0].Defense(damage);
        return 1;
    }

    //计算实际攻击力
    protected double Count_atk()
    {
        double atk = _atk;
        //TODO: 根据buff增减攻击力
        return atk;
    }

    //计算理论伤害值
    protected double Count_damage(double damage)
    {
        //TODO: 根据buff增减伤害
        return damage;
    }

    //获取攻击目标
    protected Character[] Get_target( /*敌阵列表*/)
    {
        //TODO: 将敌阵列表排序，优先级最高的在第一位
        return null;
    }

    //受攻击，调用动画、效果等
    public void Defense(double damage)
    {
        _hp = Math.Max(0, _hp - Count_Hurt(damage));
        if (_hp == 0)
        {
            Die();
        }
    }

    //计算实际伤害值
    protected double Count_Hurt(double damage)
    {
        double def = Count_def();
        damage *= (100 - def) / 100;
        //TODO: 根据buff增减实际受伤
        return damage;
    }

    //计算实际防御值
    protected double Count_def()
    {
        double def = _def;
        //TODO: 根据buff增减实际防御
        def = Math.Min(def, 75); //无论怎么加，防御最高为75 （即只受四分之一的伤害）
        return def;
    }

    //获得治疗
    public void Get_heal(double heal)
    {
        //TODO: 根据buff增减获得治疗值
        _hp = Math.Min(_maxHp, _hp + heal);
    }

    //得别人给的buff
    public void Get_buff(Buff buff)
    {
        _buffs.Add(buff);
    }

    //释放技能，该部分根据各个单位的子类具体实现
    protected abstract int Skill();

    //退场动画，效果等
    protected abstract void Die();
}
