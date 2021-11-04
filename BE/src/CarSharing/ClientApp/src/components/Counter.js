import React, { Component } from 'react';
import axios from 'axios';
import ReactPaginate from 'react-paginate';
import './Counter.scss';
import { Link } from "react-router-dom";

export class Counter extends Component {
  static displayName = Counter.name;

    constructor(props) {
        super(props);
        this.state = {
            offset: 0,
            data: [],
            perPage: 2,
            currentPage: 0
        };
        this.handlePageClick = this.handlePageClick.bind(this);
    }

    receivedData() {
        axios
            .get(`api/GetAllCars`)
            .then(res => {

                const data = res.data;
                const slice = data.slice(this.state.offset, this.state.offset + this.state.perPage)
                const postData = slice.map(cars => <React.Fragment>
                    
                <div className="car-block">
                        
                    <div className={"car-content car-block " + (cars.isAvalable ? "" : "car-not-avalable")} href="">
                            <div className="car-image">
                                <Link
                                    to={{
                                        pathname: `/details/${cars.id}`,
                                    }}>
                                    <img src={cars.carImage} />
                                </Link>
                        </div>

                         <div className="car-text">
                            <Link
                                to={{
                                  pathname: `/details/${cars.id}`,
                                 }}>
                                    <h2>{cars.title + (cars.isAvalable ? "" : " | Not avalable")}</h2>
                                </Link>

                            <p className="car-price"> $ {cars.pricePerDay}/day</p>
                            <div className="data-row">
                                <p>Transmission</p>
                                <p>{cars.transmission}</p>
                            </div>

                            <div className="data-row">
                                <p>Fuel</p>
                                <p>{cars.fuel}</p>
                            </div>

                            <div className="data-row">
                                <p>Kilometers</p>
                                <p>{cars.kilometers + (cars.isAvalable ? "" : " +")}</p>
                            </div>

                            <div className="data-row">
                                <p>Seats</p>
                                <p>{cars.seats}</p>
                            </div>
                        </div>
                    </div>
                </div>
                        
                </React.Fragment>)

                this.setState({
                    pageCount: Math.ceil(data.length / this.state.perPage),

                    postData
                })
            });
    }

    handlePageClick = (e) => {
        const selectedPage = e.selected;
        const offset = selectedPage * this.state.perPage;

        this.setState({
            currentPage: selectedPage,
            offset: offset
        }, () => {
            this.receivedData()
        });

    };

    componentDidMount() {
        this.receivedData()
    }

  render() {
    return (
        <div>
            {this.state.postData}
            <ReactPaginate
                previousLabel={"<"}
                nextLabel={">"}
                breakLabel={"..."}
                breakClassName={"break-me"}
                pageCount={this.state.pageCount}
                marginPagesDisplayed={2}
                pageRangeDisplayed={5}
                onPageChange={this.handlePageClick}
                containerClassName={"pagination"}
                subContainerClassName={"pages pagination"}
                activeClassName={"active"} />
        </div>
    );
  }
}
