using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actions {
	public static Action EMPTY = new EmptyAction();
	public static Action DASH = new DashAction();
	public static Action ATTACK = new AttackAction();
	public static Action DODGE = new DodgeAction();
}
