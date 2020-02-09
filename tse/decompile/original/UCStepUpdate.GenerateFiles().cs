public bool GnerateFiles () {
	try {
		if (this.isVisual) {
			this.rtbOperationLog.AppendText ("\n\tایجاد فایلهای خروجی ... ");
			this.rtbOperationLog.SelectionStart = this.rtbOperationLog.Text.Length;
			this.rtbOperationLog.ScrollToCaret ();
			this.progressBar.Value = 0;
			this.lblProgress.Text = "0%";
			Application.DoEvents ();
		}
		// ISSUE: object of a compiler-generated type is created
		// ISSUE: variable of a compiler-generated type
		Settings settings = new Settings ();
		string path = settings.AdjustPricesCondition != 0 ? settings.AdjustedStorageLocation : settings.StorageLocation;
		if (string.IsNullOrEmpty (path) || !Directory.Exists (path)) {
			if (this.isVisual) {
				this.rtbOperationLog.AppendText ("\n\tمقدار فیلد محل ذخیره فایل ها صحیح نمی باشد ");
				this.rtbOperationLog.SelectionStart = this.rtbOperationLog.Text.Length;
				this.rtbOperationLog.ScrollToCaret ();
			}
			return false;
		}
		int startDeven = 0;
		settings.StartDate.Replace ("/", "").ToString ();
		DateTime dateTime = Utility.ConvertJalaliStringToDateTime (settings.StartDate);
		startDeven = dateTime.Year * 10000 + dateTime.Month * 100 + dateTime.Day;
		int num1 = 0;
		using (List<string>.Enumerator enumerator = StaticData.SelectedInstruments.GetEnumerator ()) {
			while (enumerator.MoveNext ()) {
				string item = enumerator.Current;
				List<ClosingPriceInfo> cp = FileService.ClosingPrices (Convert.ToInt64 (item));
				cp = cp.FindAll ((Predicate<ClosingPriceInfo>) (p => p.DEven >= startDeven));
				if ((settings.AdjustPricesCondition == 1 || settings.AdjustPricesCondition == 2) && cp.Count > 1) {
					List<ClosingPriceInfo> closingPriceInfoList = new List<ClosingPriceInfo> ();
					Decimal num2 = new Decimal (1);
					closingPriceInfoList.Add (cp[cp.Count - 1]);
					double num3 = 0.0;
					if (settings.AdjustPricesCondition == 1) {
						for (int index = cp.Count - 2; index >= 0; --index) {
							if (cp[index].PClosing != cp[index + 1].PriceYesterday)
								++num3;
						}
					}
					if (settings.AdjustPricesCondition == 1 &&
							num3 / (double) cp.Count < 0.08 || settings.AdjustPricesCondition == 2) {
						for (int i = cp.Count - 2; i >= 0; --i) {
							if (settings.AdjustPricesCondition == 1 &&
									cp[i].PClosing != cp[i + 1].PriceYesterday)
								num2 = num2 * cp[i + 1].PriceYesterday / cp[i].PClosing;
							else if (settings.AdjustPricesCondition == 2 &&
								cp[i].PClosing != cp[i + 1].PriceYesterday &&
								StaticData.TseShares.Exists ((Predicate<TseShareInfo>) (p => {
									if (p.InsCode.ToString ().Equals (item)) return p.DEven == cp[i + 1].DEven;
									return false;
							})))
								num2 *= StaticData.TseShares.Find ((Predicate<TseShareInfo>) (p => {
									if (p.InsCode.ToString ().Equals (item)) return p.DEven == cp[i + 1].DEven;
									return false;
								})).NumberOfShareOld / StaticData.TseShares.Find ((Predicate<TseShareInfo>) (p => {
									if (p.InsCode.ToString ().Equals (item))
										return p.DEven == cp[i + 1].DEven;
									return false;
								})).NumberOfShareNew;
							closingPriceInfoList.Add (new ClosingPriceInfo () {
								InsCode = cp[i].InsCode,
									DEven = cp[i].DEven,
									PClosing = Math.Round (num2 * cp[i].PClosing, 2),
									PDrCotVal = Math.Round (num2 * cp[i].PDrCotVal, 2),
									ZTotTran = cp[i].ZTotTran,
									QTotTran5J = cp[i].QTotTran5J,
									QTotCap = cp[i].QTotCap,
									PriceMin = Math.Round (num2 * cp[i].PriceMin),
									PriceMax = Math.Round (num2 * cp[i].PriceMax),
									PriceYesterday = Math.Round (num2 * cp[i].PriceYesterday),
									PriceFirst = Math.Round (num2 * cp[i].PriceFirst, 2)
							});
						}
						cp.Clear ();
						for (int index = closingPriceInfoList.Count - 1; index >= 0; --index)
							cp.Add (closingPriceInfoList[index]);
					}
				}
				InstrumentInfo instrument = StaticData.Instruments.Find ((Predicate<InstrumentInfo>) (p => p.InsCode.ToString ().Equals (item)));
				if (!settings.ExcelOutput)
					FileService.WriteOutputFile (instrument, cp, !this.chkRemoveOldFiles.Checked);
				else
					FileService.WriteOutputExcel (instrument, cp);
				++num1;
				if (this.isVisual) {
					this.progressBar.Value = Convert.ToInt32 ((double) num1 / (double) StaticData.SelectedInstruments.Count * 100.0);
					this.lblProgress.Text = Convert.ToInt32 ((double) num1 / (double) StaticData.SelectedInstruments.Count * 100.0).ToString () + "%";
					this.rtbOperationLog.AppendText ("\n\t\t " + instrument.Symbol + " (" + instrument.Name + ")");
					this.rtbOperationLog.SelectionStart = this.rtbOperationLog.Text.Length;
					this.rtbOperationLog.ScrollToCaret ();
					Application.DoEvents ();
				}
			}
		}
		if (this.isVisual) {
			this.rtbOperationLog.AppendText ("\n\tعملیات ایجاد فایلهای خروجی با موفقیت انجام گردید.");
			this.rtbOperationLog.SelectionStart = this.rtbOperationLog.Text.Length;
			this.rtbOperationLog.ScrollToCaret ();
		}
		return true;
	} catch (Exception ex) {
		if (this.isVisual) {
			this.rtbOperationLog.AppendText ("\n\t");
			this.rtbOperationLog.AppendText ("\n\tعملیات ایجاد فایلهای خروجی ناموفق بود. ");
			this.rtbOperationLog.SelectionStart = this.rtbOperationLog.Text.Length;
			this.rtbOperationLog.ScrollToCaret ();
		}
		if (this.isVisual)
			this.rtbOperationLog.AppendText (ServerMethods.LogError ("UpdateClosingPrices", ex));
		else
			ServerMethods.LogError ("UpdateClosingPrices", ex);
		try {
			if (FileService.LogErrorFile ("[ Generating Output Files (" + StaticData.Version + ") ] " + ex.Message + "(" + (ex.InnerException != null ? ex.InnerException.Message ?? "" : "") + ")") == -1) {
				if (this.isVisual) {
					int num = (int) MessageBox.Show ("مقدار فیلد محل ذخیره فایل ها صحیح نمی باشد ");
				}
			}
		} catch {
			if (this.isVisual)
				this.rtbOperationLog.AppendText ("\n\tثبت خطا در فایل ناموفق بود");
		}
		return false;
	}
}