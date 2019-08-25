using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;



namespace DTE33
{
	[GeneratedCode("wsdl", "4.0.30319.1"), DesignerCategory("code"), System.Diagnostics.DebuggerStepThrough, WebServiceBinding(Name = "CrSeedSoapBinding", Namespace = "https://palena.sii.cl/DTEWS/CrSeed.jws")]
	public class CrSeedService : SoapHttpClientProtocol
	{
		private System.Threading.SendOrPostCallback getStateOperationCompleted;

		private System.Threading.SendOrPostCallback getSeedOperationCompleted;

		private System.Threading.SendOrPostCallback getVersionMayorOperationCompleted;

		private System.Threading.SendOrPostCallback getVersionMenorOperationCompleted;

		private System.Threading.SendOrPostCallback getVersionPatchOperationCompleted;

		public event getStateCompletedEventHandler getStateCompleted;

		public event getSeedCompletedEventHandler getSeedCompleted;

		public event getVersionMayorCompletedEventHandler getVersionMayorCompleted;

		public event getVersionMenorCompletedEventHandler getVersionMenorCompleted;

		public event getVersionPatchCompletedEventHandler getVersionPatchCompleted;

		public CrSeedService()
		{
			base.Url = "https://palena.sii.cl/DTEWS/CrSeed.jws";
		}

		[SoapRpcMethod("", RequestNamespace = "http://DefaultNamespace", ResponseNamespace = "https://palena.sii.cl/DTEWS/CrSeed.jws")]
		[return: SoapElement("getStateReturn")]
		public string getState()
		{
			object[] results = base.Invoke("getState", new object[0]);
			return (string)results[0];
		}

		public System.IAsyncResult BegingetState(System.AsyncCallback callback, object asyncState)
		{
			return base.BeginInvoke("getState", new object[0], callback, asyncState);
		}

		public string EndgetState(System.IAsyncResult asyncResult)
		{
			object[] results = base.EndInvoke(asyncResult);
			return (string)results[0];
		}

		public void getStateAsync()
		{
			this.getStateAsync(null);
		}

		public void getStateAsync(object userState)
		{
			if (this.getStateOperationCompleted == null)
			{
				this.getStateOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetStateOperationCompleted);
			}
			base.InvokeAsync("getState", new object[0], this.getStateOperationCompleted, userState);
		}

		private void OngetStateOperationCompleted(object arg)
		{
			if (this.getStateCompleted != null)
			{
				InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
				this.getStateCompleted(this, new getStateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
			}
		}

		[SoapRpcMethod("", RequestNamespace = "http://DefaultNamespace", ResponseNamespace = "https://palena.sii.cl/DTEWS/CrSeed.jws")]
		[return: SoapElement("getSeedReturn")]
		public string getSeed()
		{
			object[] results = base.Invoke("getSeed", new object[0]);
			return (string)results[0];
		}

		public System.IAsyncResult BegingetSeed(System.AsyncCallback callback, object asyncState)
		{
			return base.BeginInvoke("getSeed", new object[0], callback, asyncState);
		}

		public string EndgetSeed(System.IAsyncResult asyncResult)
		{
			object[] results = base.EndInvoke(asyncResult);
			return (string)results[0];
		}

		public void getSeedAsync()
		{
			this.getSeedAsync(null);
		}

		public void getSeedAsync(object userState)
		{
			if (this.getSeedOperationCompleted == null)
			{
				this.getSeedOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetSeedOperationCompleted);
			}
			base.InvokeAsync("getSeed", new object[0], this.getSeedOperationCompleted, userState);
		}

		private void OngetSeedOperationCompleted(object arg)
		{
			if (this.getSeedCompleted != null)
			{
				InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
				this.getSeedCompleted(this, new getSeedCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
			}
		}

		[SoapRpcMethod("", RequestNamespace = "http://DefaultNamespace", ResponseNamespace = "https://palena.sii.cl/DTEWS/CrSeed.jws")]
		[return: SoapElement("getVersionMayorReturn")]
		public string getVersionMayor()
		{
			object[] results = base.Invoke("getVersionMayor", new object[0]);
			return (string)results[0];
		}

		public System.IAsyncResult BegingetVersionMayor(System.AsyncCallback callback, object asyncState)
		{
			return base.BeginInvoke("getVersionMayor", new object[0], callback, asyncState);
		}

		public string EndgetVersionMayor(System.IAsyncResult asyncResult)
		{
			object[] results = base.EndInvoke(asyncResult);
			return (string)results[0];
		}

		public void getVersionMayorAsync()
		{
			this.getVersionMayorAsync(null);
		}

		public void getVersionMayorAsync(object userState)
		{
			if (this.getVersionMayorOperationCompleted == null)
			{
				this.getVersionMayorOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetVersionMayorOperationCompleted);
			}
			base.InvokeAsync("getVersionMayor", new object[0], this.getVersionMayorOperationCompleted, userState);
		}

		private void OngetVersionMayorOperationCompleted(object arg)
		{
			if (this.getVersionMayorCompleted != null)
			{
				InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
				this.getVersionMayorCompleted(this, new getVersionMayorCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
			}
		}

		[SoapRpcMethod("", RequestNamespace = "http://DefaultNamespace", ResponseNamespace = "https://palena.sii.cl/DTEWS/CrSeed.jws")]
		[return: SoapElement("getVersionMenorReturn")]
		public string getVersionMenor()
		{
			object[] results = base.Invoke("getVersionMenor", new object[0]);
			return (string)results[0];
		}

		public System.IAsyncResult BegingetVersionMenor(System.AsyncCallback callback, object asyncState)
		{
			return base.BeginInvoke("getVersionMenor", new object[0], callback, asyncState);
		}

		public string EndgetVersionMenor(System.IAsyncResult asyncResult)
		{
			object[] results = base.EndInvoke(asyncResult);
			return (string)results[0];
		}

		public void getVersionMenorAsync()
		{
			this.getVersionMenorAsync(null);
		}

		public void getVersionMenorAsync(object userState)
		{
			if (this.getVersionMenorOperationCompleted == null)
			{
				this.getVersionMenorOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetVersionMenorOperationCompleted);
			}
			base.InvokeAsync("getVersionMenor", new object[0], this.getVersionMenorOperationCompleted, userState);
		}

		private void OngetVersionMenorOperationCompleted(object arg)
		{
			if (this.getVersionMenorCompleted != null)
			{
				InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
				this.getVersionMenorCompleted(this, new getVersionMenorCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
			}
		}

		[SoapRpcMethod("", RequestNamespace = "http://DefaultNamespace", ResponseNamespace = "https://palena.sii.cl/DTEWS/CrSeed.jws")]
		[return: SoapElement("getVersionPatchReturn")]
		public string getVersionPatch()
		{
			object[] results = base.Invoke("getVersionPatch", new object[0]);
			return (string)results[0];
		}

		public System.IAsyncResult BegingetVersionPatch(System.AsyncCallback callback, object asyncState)
		{
			return base.BeginInvoke("getVersionPatch", new object[0], callback, asyncState);
		}

		public string EndgetVersionPatch(System.IAsyncResult asyncResult)
		{
			object[] results = base.EndInvoke(asyncResult);
			return (string)results[0];
		}

		public void getVersionPatchAsync()
		{
			this.getVersionPatchAsync(null);
		}

		public void getVersionPatchAsync(object userState)
		{
			if (this.getVersionPatchOperationCompleted == null)
			{
				this.getVersionPatchOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetVersionPatchOperationCompleted);
			}
			base.InvokeAsync("getVersionPatch", new object[0], this.getVersionPatchOperationCompleted, userState);
		}

		private void OngetVersionPatchOperationCompleted(object arg)
		{
			if (this.getVersionPatchCompleted != null)
			{
				InvokeCompletedEventArgs invokeArgs = (InvokeCompletedEventArgs)arg;
				this.getVersionPatchCompleted(this, new getVersionPatchCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
			}
		}

		public new void CancelAsync(object userState)
		{
			base.CancelAsync(userState);
		}
	}
}
