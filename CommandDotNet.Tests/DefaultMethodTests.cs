﻿using CommandDotNet.Attributes;
using CommandDotNet.Exceptions;
using CommandDotNet.Models;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace CommandDotNet.Tests
{
    public class DefaultMethodTests : TestBase
    {
        public DefaultMethodTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void CanExecuteDefaultMethodWithoutConstructor()
        {
            AppRunner<DefaultMethodTestAppWithoutContructor> appRunner = new AppRunner<DefaultMethodTestAppWithoutContructor>();
            int result = appRunner.Run(new string[]{});
            result.Should().Be(10, "return value of default method is 10");
        }

        [Fact]
        public void CanExecuteDefaultMethodWithConstructor()
        {
            AppRunner<DefaultMethodTestAppWithContructor> appRunner = new AppRunner<DefaultMethodTestAppWithContructor>(new AppSettings
            {
                MethodArgumentMode = ArgumentMode.Option
            });
            int result = appRunner.Run(new []{"--exitCode", "20"});
            result.Should().Be(20, "return value of default method is 20");
        }

        [Fact]
        public void ShouldThrowErrorWhenDefaultMethodHasParameters()
        {
            AppRunner<DefaultMethodWithParametersTest> appRunner = new AppRunner<DefaultMethodWithParametersTest>();
            int result = appRunner.Run(new string[]{});
            result.Should().Be(1, "application should throw error");
        }
    }

    public class DefaultMethodTestAppWithoutContructor
    {
        [DefaultMethod]        
        public int DefaultMethod()
        {
            return 10;
        }
    }
    
    public class DefaultMethodWithParametersTest
    {
        [DefaultMethod]        
        public void DefaultMethod(int value)
        {
        }
    }
    
    public class DefaultMethodTestAppWithContructor
    {
        private readonly int _exitCode;

        public DefaultMethodTestAppWithContructor(int exitCode)
        {
            _exitCode = exitCode;
        }
        
        [DefaultMethod]        
        public int DefaultMethod()
        {
            return _exitCode;
        }
    }
}