using PipeVolumeCalcService;
using System;
using System.Runtime.Serialization;
using System.ServiceModel;


[ServiceContract(SessionMode = SessionMode.Required)]
public interface IPipeVolumeCalculator
{
	[OperationContract]
	[FaultContract(typeof(ArgumentException))]
	double CalculatePipeVolume(double L, double S);

	[OperationContract]
	[FaultContract(typeof(FaultException))]
	bool Authorize(User user);

	[OperationContract]
	[FaultContract(typeof(FaultException))]
	CompositeType GetDataUsingDataContract(CompositeType composite);

}

[DataContract]
public class CompositeType
{
	double L;
	double S;
	double V;

	[DataMember]
	public double ParamValueL
	{
		get { return L; }
		set { L = value; }
	}

	[DataMember]
	public double ParamValueS
	{
		get { return S; }
		set { S = value; }
	}

	[DataMember]
	public double ParamValueV
	{
		get { return V; }
		set { V = value; }
	}
}
