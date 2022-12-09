using PipeVolumeCalcService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
public class PipeVolumeCalculationService: IPipeVolumeCalculator
{
	public static bool authenticatedSession;
	public static string user;

	private string _argExMsg = "Ошибка!!! Аргумент/аргументы не корректны!";
	private string _notAurhorizeExMsg = "Ошибка!!! Пользователь не авторизован!";
	private string _errAurhorizeDataExMsg = "Ошибка!!! Несанкционированный доступ! (Автризация не уалась из-за неверных входных данных)";
	private string _usersDB = "usersDB.csv";

	private DBReader dBReader;

	public double CalculatePipeVolume(double L, double S)
	{
		if(authenticatedSession)
        {
			try
			{
				if (L > 0 && S > 0)
				{
					return L * S;
				}
				throw new ArgumentException(_argExMsg);
			}
			catch (Exception e)
			{
				throw new ArgumentException(e.Message);
			}
		}
        else
        {
			throw new Exception(_notAurhorizeExMsg);
        }
	}

	public bool Authorize(User user)
	{
		dBReader = new DBReader(_usersDB, new DBTypeCSVReader());
		dBReader.ReadDB();

		if (dBReader.DBTypeReader.IsValidUser(user))
        {
			authenticatedSession = true;
			return true;
        }

		throw new Exception(_errAurhorizeDataExMsg);
	}

	public CompositeType GetDataUsingDataContract(CompositeType composite)
	{
		if (composite == null)
		{
			throw new ArgumentNullException(_argExMsg);
		}
		if (composite.ParamValueL > 0 && composite.ParamValueS > 0)
		{
			composite.ParamValueV = composite.ParamValueL * composite.ParamValueS;
		}
		return composite;
	}
}
