/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/4
 * Time: 20:38
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Generic trap code.
    /// </summary>
	public enum GenericCode
	{
        /// <summary>
        /// Indicates that the agent has rebooted. All management variables will be reset; specifically, Counters and Gauges will be reset to zero (0). One nice thing about the coldStart trap is that it can be used to determine when new hardware is added to the network. When a device is powered on, it sends this trap to its trap destination. If the trap destination is set correctly (i.e., to the IP address of your NMS) the NMS can receive the trap and determine whether it needs to manage the device.
        /// </summary>
		ColdStart = 0,
        /// <summary>
        /// Indicates that the agent has reinitialized itself. None of the management variables will be reset.
        /// </summary>
		WarmStart = 1,
        /// <summary>
        /// Sent when an interface on a device goes down. The first variable binding identifies which interface went down.
        /// </summary>
		LinkDown = 2,
        /// <summary>
        /// Sent when an interface on a device comes back up. The first variable binding identifies which interface came back up.
        /// </summary>
		LinkUp = 3,
        /// <summary>
        /// Indicates that someone has tried to query your agent with an incorrect community string; useful in determining if someone is trying to gain unauthorized access to one of your devices.
        /// </summary>
		AuthenticationFailure = 4,
        /// <summary>
        /// Indicates that an Exterior Gateway Protocol (EGP) neighbor has gone down.
        /// </summary>
		EgpNeighborLoss = 5,
        /// <summary>
        /// Indicates that the trap is enterprise-specific. SNMP vendors and users define their own traps under the private-enterprise branch of the SMI object tree. To process this trap properly, the NMS has to decode the specific trap number that is part of the SNMP message.
        /// </summary>
		EnterpriseSpecific = 6
	}
}
