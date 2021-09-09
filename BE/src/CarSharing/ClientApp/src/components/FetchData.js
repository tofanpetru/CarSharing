import React, { Component } from 'react';
import authService from './api-authorization/AuthorizeService'

export class FetchData extends Component {
  static displayName = FetchData.name;

  constructor(props) {
    super(props);
    this.state = { cars: [], loading: true };
  }

  componentDidMount() {
    this.populateCarData();
  }

  static renderCarsTable(cars) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Date</th>
            <th>Title</th>
            <th>Brand</th>
          </tr>
        </thead>
        <tbody>
          {cars.map(car =>
              <tr key={car.publishDate}>
              <td>{car.publishDate}</td>
              <td>{car.title}</td>
              <td>{car.carbrand}</td>

            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : FetchData.renderCarsTable(this.state.cars);

    return (
      <div>
        <h1 id="tabelLabel" >Weather forecast</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }

  async populateCarData() {
    const token = await authService.getAccessToken();
    const response = await fetch('car', {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });
    const data = await response.json();
    this.setState({ cars: data, loading: false });
  }
}
