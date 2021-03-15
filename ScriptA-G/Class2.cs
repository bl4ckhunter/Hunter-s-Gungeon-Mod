using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ClassLibrary1.Scripts
{
	public class AutoGunBehaviour : MonoBehaviour
	{
		// Token: 0x060001ED RID: 493 RVA: 0x00016ADC File Offset: 0x00014CDC
		private void Start()
		{
			this.gun = base.GetComponent<Gun>();
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00016AEC File Offset: 0x00014CEC
		private void Update()
		{
			foreach (ProjectileModule projectileModule in this.gun.Volley.projectiles)
			{
				bool flag = projectileModule.shootStyle != ProjectileModule.ShootStyle.Automatic && projectileModule.shootStyle != ProjectileModule.ShootStyle.Charged && projectileModule.shootStyle != ProjectileModule.ShootStyle.Beam;
				if (flag)
				{
					bool flag2 = this.shootStyles.ContainsKey(projectileModule);
					if (flag2)
					{
						bool flag3 = this.shootStyles[projectileModule] != projectileModule.shootStyle;
						if (flag3)
						{
							this.shootStyles[projectileModule] = projectileModule.shootStyle;
						}
					}
					else
					{
						this.shootStyles.Add(projectileModule, projectileModule.shootStyle);
					}
					projectileModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
				}
			}
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00016BD8 File Offset: 0x00014DD8
		private void ForwardToPast()
		{
			foreach (KeyValuePair<ProjectileModule, ProjectileModule.ShootStyle> keyValuePair in this.shootStyles)
			{
				keyValuePair.Key.shootStyle = keyValuePair.Value;
			}
			this.shootStyles.Clear();
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00016C48 File Offset: 0x00014E48
		public void Destroy()
		{
			this.ForwardToPast();
			UnityEngine.Object.Destroy(this);
		}

		// Token: 0x040002B9 RID: 697
		private Gun gun;

		// Token: 0x040002BA RID: 698
		private Dictionary<ProjectileModule, ProjectileModule.ShootStyle> shootStyles = new Dictionary<ProjectileModule, ProjectileModule.ShootStyle>();
	}
}