using System;
using UnityEngine;

namespace Code.Infrastructure.Data.Vfx
{
	[Serializable]
	public struct VfxData
	{
		public string Name;
		public VFXType VfxType;
		public ParticleSystem Vfx; //TODO(vlad): Could Be Array To Pick Random?
	}
}