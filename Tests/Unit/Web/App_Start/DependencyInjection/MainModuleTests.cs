﻿using MvcTemplate.Components.Logging;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Services;
using MvcTemplate.Web.DependencyInjection;
using Ninject;
using NUnit.Framework;
using System;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Web.DependencyInjection
{
    [TestFixture]
    public class MainModuleTests
    {
        private KernelBase kernel;
        private MainModule module;

        [SetUp]
        public void SetUp()
        {
            module = new MainModule();
            kernel = new StandardKernel(module);
        }

        #region Method: Load()

        [Test]
        public void Load_BindsILogger()
        {
            AssertBind<ILogger, Logger>();
        }

        [Test]
        public void Load_BindsAContext()
        {
            AssertBind<AContext, Context>();
        }

        [Test]
        public void Load_BindsIUnitOfWork()
        {
            AssertBind<IUnitOfWork, UnitOfWork>();
        }

        [Test]
        public void Load_BindsIEntityLogger()
        {
            AssertBind<IEntityLogger, EntityLogger>();
        }

        [Test]
        public void Load_BindsIExceptionFilter()
        {
            AssertBind<IExceptionFilter, ExceptionFilter>();
        }

        [Test]
        public void Load_BindsIRoleProvider()
        {
            AssertBind<IRoleProvider, RoleProvider>();
        }

        [Test]
        public void Load_BindsIRoleProviderToConstant()
        {
            IRoleProvider expected = kernel.Get<IRoleProvider>();
            IRoleProvider actual = kernel.Get<IRoleProvider>();

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void Load_BindsIMvcSiteMapParser()
        {
            AssertBind<IMvcSiteMapParser, MvcSiteMapParser>();
        }

        [Test]
        [Ignore]
        public void Load_BindsIMvcSiteMapProvider()
        {
            AssertBind<IMvcSiteMapProvider, MvcSiteMapProvider>();
        }

        [Test]
        public void Load_BindsIHasher()
        {
            AssertBind<IHasher, BCrypter>();
        }

        [Test]
        public void Load_BindsIRolesService()
        {
            AssertBind<IRolesService, RolesService>();
        }

        [Test]
        public void Load_BindsIAccountsService()
        {
            AssertBind<IAccountsService, AccountsService>();
        }

        #endregion

        #region Test helpers

        private void AssertBind<TAbstraction, TImplementation>()
        {
            Type actual = kernel.Get<TAbstraction>().GetType();
            Type expected = typeof(TImplementation);

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
