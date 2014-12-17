﻿// ****************************************************************************
// <copyright file="INavigationService.cs" company="Pedro Lamas">
// Copyright © Pedro Lamas 2011
// </copyright>
// ****************************************************************************
// <author>Pedro Lamas</author>
// <email>pedrolamas@gmail.com</email>
// <date>17-11-2011</date>
// <project>Cimbalino.Phone.Toolkit</project>
// <web>http://www.pedrolamas.com</web>
// <license>
// See license.txt in this solution or http://www.pedrolamas.com/license_MIT.txt
// </license>
// ****************************************************************************

using System;
using System.Collections.Generic;
using System.Windows.Navigation;

namespace WpWinNl.Utilities
{
  /// <summary>
  /// Represents a service capable of handling the application navigation.
  /// </summary>
  public interface INavigationService
  {
    /// <summary>
    /// Occurs when a new navigation is requested.
    /// </summary>
    event NavigatingCancelEventHandler Navigating;

    /// <summary>
    /// Gets the uniform resource identifier (URI) of the content that is currently displayed.
    /// </summary>
    /// <value>Returns a value that represents the <see cref="Uri"/> of content that is currently displayed.</value>
    Uri CurrentSource { get; }

    /// <summary>
    /// Gets a collection of query string values.
    /// </summary>
    /// <value>Returns a <see cref="IDictionary{String,String}"/> collection that contains the query string values.</value>
    IDictionary<string, string> QueryString { get; }

    /// <summary>
    /// Navigates to the content specified by the uniform resource identifier (URI).
    /// </summary>
    /// <param name="source">The URI for the desired content.</param>
    void NavigateTo(string source);

    /// <summary>
    /// Navigates to the content specified by the uniform resource identifier (URI).
    /// </summary>
    /// <param name="source">A <see cref="Uri"/> initialized with the URI for the desired content.</param>
    void NavigateTo(Uri source);

    /// <summary>
    /// Gets a value indicating whether there is at least one entry in the back navigation history.
    /// </summary>
    /// <value>true if there is at least one entry in the back navigation history; otherwise, false.</value>
    bool CanGoBack { get; }

    /// <summary>
    /// Navigates to the most recent available entry in the back navigation history.
    /// </summary>
    void GoBack();

    /// <summary>
    /// Removes the most recent available entry from the back stack.
    /// </summary>
    void RemoveBackEntry();
  }
}
