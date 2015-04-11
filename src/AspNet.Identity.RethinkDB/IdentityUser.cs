﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace AspNet.Identity.RethinkDB
{
	[DataContract]
	public class IdentityUser : IUser<string>
	{
		public IdentityUser()
		{
			Id = Guid.NewGuid().ToString("N");
			Roles = new List<string>();
			Logins = new List<UserLoginInfo>();
			Claims = new List<IdentityUserClaim>();
		}

		// TODO: Make private set. But: if that is private set, an Exception in RethinkDB-driver DataContractDatumConverterFactory occurs.
		private string id;

		[DataMember]
		public string Id
		{
			get
			{
				return id;
			}

			set
			{
				if (value != null)
					id = value;
			}
		}

		[DataMember]
		public string UserName { get; set; }

		[DataMember]
		public virtual string SecurityStamp { get; set; }

		[DataMember]
		public virtual string Email { get; set; }

		[DataMember]
		public virtual bool EmailConfirmed { get; set; }

		[DataMember]
		public virtual string PhoneNumber { get; set; }

		[DataMember]
		public virtual bool PhoneNumberConfirmed { get; set; }

		[DataMember]
		public virtual bool TwoFactorEnabled { get; set; }

		[DataMember]
		public virtual DateTime? LockoutEndDateUtc { get; set; }

		[DataMember]
		public virtual bool LockoutEnabled { get; set; }

		[DataMember]
		public virtual int AccessFailedCount { get; set; }

		[DataMember]
		public List<string> Roles { get; set; }

		public virtual void AddRole(string role)
		{
			Roles.Add(role);
		}

		public virtual void RemoveRole(string role)
		{
			Roles.Remove(role);
		}

		[DataMember]
		public virtual string PasswordHash { get; set; }

		//[DataMember]
		public List<UserLoginInfo> Logins { get; set; }

		public virtual void AddLogin(UserLoginInfo login)
		{
			Logins.Add(login);
		}

		public virtual void RemoveLogin(UserLoginInfo login)
		{
			var loginsToRemove = Logins
				.Where(l => l.LoginProvider == login.LoginProvider)
				.Where(l => l.ProviderKey == login.ProviderKey);

			Logins = Logins.Except(loginsToRemove).ToList();
		}

		public virtual bool HasPassword()
		{
			return false;
		}

		public List<IdentityUserClaim> Claims { get; set; }

		public virtual void AddClaim(Claim claim)
		{
			Claims.Add(new IdentityUserClaim(claim));
		}

		public virtual void RemoveClaim(Claim claim)
		{
			var claimsToRemove = Claims
				.Where(c => c.Type == claim.Type)
				.Where(c => c.Value == claim.Value);

			Claims = Claims.Except(claimsToRemove).ToList();
		}
	}
}